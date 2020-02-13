using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Visionary.NET;
using Visionary.NET.Shared;
using Visionary.NET.Shared.Models;
using Visionary.NET.Shared.Lib;
using Visionary.NET.Shared.Models.SickRecord;
using Visionary.NET.Shared.PointCloud;
using System.Numerics;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //global var settings
        bool AngleCorr = true;
        bool RGBAsZmap = false;
        int MinPixelArea = 2000;
        ushort Zmap_DR;
        ushort Zmap_Offset;
        double RatioFilter;
        double BackgroundH=1.5;
        double Rounding = 1;
        int ROIx=0;
        int ROIy=0;
        double ROIScale=1;

        //misc
        int index = 0;      //file index
        Bitmap bitmap;      //Zmap bitmap
        Bitmap bitmap_RGB;  //RGB bitmap
        Bitmap bitmap_Mixed;//mixed bitmap
        byte[] bitmap_arry; 
        ushort[] ZMap_arry;
        float CenterH;

        //internal flags
        bool SaveQueueList = false;
        bool SaveQueuePly = false;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //find current .exe path
            textBox_Savepath.Text = AppDomain.CurrentDomain.BaseDirectory;
        }
        
        //connect to visionary
        private void button_Connect_Click(object sender, EventArgs e)
        {
            String host = textBox_IP.Text;
            int port = 2114;
            VisionaryDataStream dataStream = new VisionaryDataStream(host, port);
            VisionaryControl control = new VisionaryControl(host);
            textBox_Index.Text = "0";
            Task.Run(async () =>
            {
                // Buffer for reading data
                Byte[] bytes = new Byte[1024];

                try
                {
                    IPAddress ipAddress = Dns.Resolve("localhost").AddressList[0];
                    TcpListener server = new TcpListener(ipAddress, Convert.ToInt32(textBox_BackendPort.Text));
                    System.Threading.Thread.Sleep(1000);
                    server.Start();
                    //MessageBox.Show("Waiting for client to connect...");
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream nStream = client.GetStream();
                    //network loop, send then wait for reply loop
                    int i = -1;
                    while (true)
                    {
                        Thread.Sleep(2);
                        i = nStream.Read(bytes, 0, bytes.Length);

                        if (i > 0 )
                        {
                            String data = null;
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            //MessageBox.Show("Received:" + data);
                            // Process the data sent by the client.
                            data = data.ToUpper();
                            //nStream.Write(bytes, 0, bytes.Length);

                            if (data == "REQUEST\n")
                            {
                                //transmit 1 frame
                                //compile image from 640x512x3 -> 983040x1
                                try
                                {
                                    //byte[] bStream = ImageToByte(bitmap_RGB);
                                    nStream.Write(bitmap_arry, 0, bitmap_arry.Length);
                                }
                                catch
                                {
                                }
                            }
                            else if (data != "EMPTY\n"&& checkBox_UseBackend.Checked == true)
                            {
                                try
                                {
                                    parsedata(data);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show("SocketException: " + e1.Message);
                }
            });

            try
            {
                Task.Run(async () =>
                {
                    if (!await dataStream.ConnectAsync())
                    {
                        // Data stream connection failed
                        MessageBox.Show("error");
                    }
                    if (!await control.ConnectAsync())
                    {
                        // Data control (CoLaB) connection failed
                    }
                    await control.StartAcquisitionAsync();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR:"+ ex.ToString());
                throw;
            }
            System.Threading.Thread.Sleep(1500);

            //-------------------------------------------------------------------------------------------------------
            // Receiving & Image Processing thread
            //-------------------------------------------------------------------------------------------------------
            Task.Run(async () =>
            {
                while (true)
                {
                    //-------------------------------------------------------------------------------------------------------
                    // Read Data from Visionary Datastream
                    //-------------------------------------------------------------------------------------------------------
                    VisionaryFrame frame = await dataStream.GetNextFrameAsync();

                    //System.Threading.Thread.Sleep(1000);
                    VisionarySDepthMapData depthMap = frame.GetData<VisionarySDepthMapData>();

                    // Important: When converting multiple frames, make sure to re-use the same converter as it will result in much better performance.
                    PointCloudConverter converter = new PointCloudConverter();
                    Vector3[] pointCloud = converter.Convert(depthMap);
                    CenterH = pointCloud[250 * 640 + 320].Z;
                    //read and set range of textboxs
                    setTextboxRange();

                    //Assign converted image
                    bitmap = depthMap.ZMap.ToBitmap(Zmap_DR, Zmap_Offset);
                    bitmap_RGB = depthMap.RgbaMap.ToBitmap();
                    bitmap_arry = depthMap.RgbaMap.Data.ToArray();
                    ZMap_arry = depthMap.ZMap.Data.ToArray();

                    this.label1.Text = bitmap.GetPixel(320, 250).R.ToString();
                    //-------------------------------------------------------------------------------------------------------
                    // Optional default image proccessing method (locate box)
                    //-------------------------------------------------------------------------------------------------------
                    if (checkBox_MinAreaRect.Checked == true)
                    {
                        Bitmap TempMap = bitmap;
                        if(RGBAsZmap==true){TempMap = bitmap_RGB;}

                        //init different images for different detection stages
                        Image<Bgr, byte> a = new Image<Bgr, byte>(TempMap);
                        Image<Gray, byte> b = new Image<Gray, byte>(a.Width, a.Height);         //edge detection
                        Image<Gray, byte> c = new Image<Gray, byte>(a.Width, a.Height);         //find contour
                        
                        //set threshold
                        int Blue_threshold = 50; //0-255
                        int Green_threshold = 50; //0-255
                        int Red_threshold = 50; //0-255
                        if (RGBAsZmap == false) 
                        {
                            a = ~a;
                            a = a.ThresholdBinary(new Bgr(Blue_threshold, Green_threshold, Red_threshold), new Bgr(255, 255, 255));
                        }
                        
                        //Set ROI
                        a.ROI =new Rectangle(ROIx ,ROIy ,(int)(640*ROIScale) , (int)(512*ROIScale));

                        //Find edges
                        int cannytherhold = 100;
                        CvInvoke.Canny(a, b, cannytherhold/2, cannytherhold,3,false);

                        //Enhance canny edges
                        Mat struct_element = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
                        CvInvoke.Dilate(b, b, struct_element, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(255, 255, 255));

                        //Find contours
                        VectorOfVectorOfPoint con = new VectorOfVectorOfPoint(); 
                        CvInvoke.FindContours(b, con, c, RetrType.List, ChainApproxMethod.ChainApproxNone);

                        Point[][] con1 = con.ToArrayOfArray();
                        PointF[][] con2 = Array.ConvertAll<Point[], PointF[]>(con1, new Converter<Point[], PointF[]>(PointToPointF));

                        listBox_BoxList.Items.Clear();
                        for (int i = 0; i < con.Size; i++)
                        {
                            //Filter params
                            double tempArea = CvInvoke.ContourArea(con[i], true);
                            double tempArc = CvInvoke.ArcLength(con[i],true);
                            double tempScale = tempArea / Math.Pow(tempArc / 4, 2);

                            if (tempArea >= MinPixelArea && tempScale > RatioFilter)
                            {
                                RotatedRect rrec = CvInvoke.MinAreaRect(con2[i]);       //g

                                //-------------------------------------------------------------------------------------------------------
                                // find box dimensions
                                //-------------------------------------------------------------------------------------------------------

                                //find box height
                                int boxHeight = -10000;
                                int tempX = 0;
                                int tempY = 0;
                                while (boxHeight<0)
                                {   
                                    boxHeight = (int)Math.Round(((double)BackgroundH - (double)pointCloud[(ROIy+tempY + (int)rrec.Center.Y) * 640 + (ROIx +tempX+ (int)rrec.Center.X)].Z) * (double)1000);
                                    tempX++;
                                }

                                //apply sensor prespective angle correction
                                if (AngleCorr == true)
                                {
                                    double boxCenterOffset = Math.Sqrt(Math.Pow(pointCloud[(ROIy+(int)rrec.Center.Y) * 640 + (ROIx+(int)rrec.Center.X)].X, 2) + Math.Pow(pointCloud[(ROIy+(int)rrec.Center.Y) * 640 + (ROIx+(int)rrec.Center.X)].Y, 2));
                                    double boxCenterAngle = Math.Atan(boxCenterOffset*1.1 / BackgroundH);
                                    double heightMulti = 1 / Math.Cos(boxCenterAngle);
                                    boxHeight = (int)((double)boxHeight * heightMulti);
                                }
                                
                                //find dimension of 1 pixel
                                double PixelScaleX = (double)(pointCloud[(int)(ROIy + rrec.Center.Y) * 640 + (int)(ROIx + rrec.Center.X) - 15].X - (double)pointCloud[(int)(ROIy + rrec.Center.Y) * 640 + (int)(ROIx + rrec.Center.X) + 15].X) * 1000.0 / 30.0;
                                double PixelScaleY = (double)(pointCloud[(int)(ROIy + rrec.Center.Y - 15) * 640 + (int)(ROIx + rrec.Center.X)].X - (double)pointCloud[(int)(ROIy + rrec.Center.Y + 15) * 640 + (int)(ROIx + rrec.Center.X)].X) * 1000.0 / 30.0;
                                double PixelScale = 0;
                                if (PixelScaleY<0)
                                {
                                    PixelScale = PixelScaleX;
                                }
                                else if (PixelScaleX<0)
                                {
                                    PixelScale = PixelScaleY;
                                }
                                else
                                {
                                    PixelScale = (PixelScaleX + PixelScaleY) / 2;
                                } 
                                int boxWidth =(int) (rrec.Size.Width * PixelScale);
                                int boxLength = (int)(rrec.Size.Height* PixelScale);

                                //Rounding result
                                boxLength = (int)(Math.Round((double)boxLength / Rounding, MidpointRounding.AwayFromZero) * Rounding);
                                boxWidth = (int)(Math.Round((double)boxWidth / Rounding, MidpointRounding.AwayFromZero) * Rounding);
                                boxHeight = (int)(Math.Round((double)boxHeight / Rounding, MidpointRounding.AwayFromZero) * Rounding);

                                double boxArea = ((double)boxLength/10) * ((double)boxWidth/10) * ((double)boxHeight/10) ;//cm

                                //add box to listbox
                                listBox_BoxList.Items.Add("Box (Length: " + boxLength+ "mm, Width: " + boxWidth + "mm, Height: " + boxHeight +"mm, Vol:" + boxArea + "cm^3)");

                                PointF[] pointfs = rrec.GetVertices();
                                for (int j = 0; j < pointfs.Length; j++)
                                    CvInvoke.Line(a, new Point((int)pointfs[j].X, (int)pointfs[j].Y), new Point((int)pointfs[(j + 1) % 4].X, (int)pointfs[(j + 1) % 4].Y), new MCvScalar(0, 255, 0, 255), 4);
                            }
                        }

                        //save box list
                        if (SaveQueueList==true)
                        {
                            System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(textBox_Savepath.Text + "/Output/Detection Result/" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt");

                            foreach (var item in listBox_BoxList.Items)
                            {
                                SaveFile.WriteLine(item);
                            }

                            SaveFile.Close();
                            SaveQueueList = false;
                        }

                        //save pointcloud
                        if (SaveQueuePly == true)
                        {
                            await PointCloudPlyWriter.WriteFormatPLYAsync(textBox_Savepath.Text + "/Output/PointCloud/"+index.ToString()+".ply", pointCloud, depthMap.RgbaMap, true);
                        }
                        /* 
                        //for displaying contours
                        for (int i = 0; i < con.Size; i++)
                        {
                            CvInvoke.DrawContours(d, con, i, new MCvScalar(255, 255, 0, 255), 2);
                        }
                        */

                        this.pictureBox2.Image = a.ToBitmap();
                        this.pictureBox1.Image = bitmap_RGB;
                    }
                    else
                    {
                        this.pictureBox2.Image = bitmap_RGB;
                        this.pictureBox1.Image = bitmap;
                    }
                    try
                    {
                        bitmap_Mixed = mixedMap(bitmap_arry, ZMap_arry, Convert.ToUInt16(textBox_DynamicRange.Text));
                        this.pictureBox_Mixed.Image = bitmap_Mixed;
                    }
                    catch { }
                }
            });
        }

        //-------------------------------------------------------------------------------------------------------
        // User Inputs
        //-------------------------------------------------------------------------------------------------------

        private void textBox_Rounding_TextChanged(object sender, EventArgs e)
        {
            if(textBox_Rounding.Text=="")
            {
                textBox_Rounding.Text = "1";
            }
            else
            {
                Rounding = Convert.ToInt32(textBox_Rounding.Text);
            }
        }
        private void trackBar_ROIx_Scroll(object sender, EventArgs e)
        {
            if (trackBar_ROIx.Value + ((double)trackBar_ROIScale.Value/10) * 640.0 < 640)
            {
                ROIx = trackBar_ROIx.Value;
            }
        }

        private void trackBar_ROIy_Scroll(object sender, EventArgs e)
        {
            if ((512-trackBar_ROIy.Value )+ ((double)trackBar_ROIScale.Value / 10) * 512.0 < 512)
            {
                ROIy = 512 - trackBar_ROIy.Value;
            }
        }

        private void trackBar_ROIScale_Scroll(object sender, EventArgs e)
        {
            ROIScale = (double)trackBar_ROIScale.Value/100.0;
        }

        private void checkBox_AngleCorr_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_AngleCorr.Checked == true)
            {AngleCorr = true;}
            else
            {AngleCorr = false;}
        }
        private void checkBox_RGBAsZmap_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_RGBAsZmap.Checked==true)
            {RGBAsZmap = true;}
            else 
            { RGBAsZmap = false; }
        }

        private void button_AutoCali_Click(object sender, EventArgs e)
        {
            textBox_BackgroundH.Text = CenterH.ToString();
            BackgroundH = CenterH;
            // pixelValue = (byte)(((image.Data[stereoOffset + x] + Zmap_Offset )/ (double)scalingMaxValue) * byte.MaxValue);
            // pixel = (byte)(((Zmap[i2]+Zmap_Offset) / (double)scalingMaxValue) * byte.MaxValue)
            // (( pixel / byte.MaxValue ) *(double)Zmap_DR)-Zmap_arry[250 * 640 + 320]

            textBox_ZmapOffset.Text = ((ushort)(((double)(255+Convert.ToInt32(textBox_OffsetSafety.Text))  / (double)byte.MaxValue) * (double)Zmap_DR - ZMap_arry[250 * 640 + 320])).ToString();
        }
        private void button_Save_Click(object sender, EventArgs e)
        {
            CreateDirectory();
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap_RGB.Save(textBox_Savepath.Text + "/Output/rgb/rgb" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }

        private void button_Save_depth_Click(object sender, EventArgs e)
        {
            CreateDirectory();
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap.Save(textBox_Savepath.Text+"/Output/dep/dep" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }

        private void button_Save_mixed_Click(object sender, EventArgs e)
        {
            CreateDirectory();
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap_Mixed.Save(textBox_Savepath.Text+"/Output/mixed/mixed" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }

        private void button_Save_all_Click(object sender, EventArgs e)
        {
            CreateDirectory();
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap_RGB.Save(textBox_Savepath.Text+"/Output/rgb/rgb" + index.ToString() + ".png");
            bitmap.Save(textBox_Savepath.Text+"/Output/dep/dep" + index.ToString() + ".png");
            bitmap_Mixed.Save(textBox_Savepath.Text+"/Output/mixed/mixed" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }
        private void button_SavePointCloud_Click(object sender, EventArgs e)
        {
            CreateDirectory();
            SaveQueuePly = true;
        }
        private void button_SaveResult_Click(object sender, EventArgs e)
        {
            CreateDirectory();
            SaveQueueList = true;
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            RatioFilter = (double)trackBar_RatioFilter.Value / 20;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome to the help page that will probably cause more confusion...\n\n" +
                "This Application can be used to detect ANY objects with backend ROI enabled\n" +
                "has built-in box detection Algorithm\n\n" +
                "General Input & Params:\n" +
                "   1. Connect(Button)\n" +
                "       - Connects to Visionary with IP next to it\n" +
                "       - set up Backend server for external detection methods using Port\n" +
                "         in Tab/Backend Detection\n" +
                "   2. IP (Textbox)\n" +
                "       - Expects IP input used for Visionary, Ex: \"192.168.1.10\"\n" +
                "   3. Help! (Button)\n" +
                "       - Leads to here\n\n" +
                "Tab Navigation:\n" +
                "   1. Box Detection Algorithm\n" +
                "       1.1 Locate Box (Checkbox):\n" +
                "           - Checking this will enable box detection\n" +
                "             and other detection settings\n" +
                "       1.2 Use RGB as Zmap (Checkbox)\n" +
                "           - Will replace Box Detection input with RGB image\n" +
                "           - Requires clear color difference for accurate detection\n" +
                "       1.3 Prespective angle correction (Checkbox)\n" +
                "           - Corrects the height Z due to box X/Y axis offset\n" +
                "           - Enable this unless it causes problem\n" +
                "       1.4 Min Pixel Area Filter (Textbox)\n" +
                "           - Filters away false detection via minimum area\n" +
                "           - Unit in Pixels area,Ex: filter 2000, 50px*20px will be filtered\n" +
                "       1.5 Detection Step Size (Textbox)\n" +
                "           - Detection Result W/L/H will round to this number\n" +
                "       1.6 Ratio Filter (Slidebar)\n" +
                "           - Filters away false detection via object ratio\n" +
                "           - set to 0 will disable filter\n" +
                "           - set to 1 will require a perfect rectangle\n" +
                "       1.7 Save Result (Button)\n" +
                "           - Saves detected box list to path in \"save image\"Tab\n" +
                "   2. Backend Detection\n" +
                "       2.1 Use Backend ROI (Checkbox)\n" +
                "           - Enables backend input, will send a frame over upon\n" +
                "             receiving \"REQUEST\\n\"\n" +
                "           - Expects a reply from backend(x,y,w,h) \n" +
                "             for each bounding box in ASCII\n" +
                "       2.2 Port (Textbox)\n" +
                "           - Port for backend detection\n" +
                "           - Set this BEFORE connecting\n" + 
                "   3. Settings\n" +
                "       3.1 Zmap Dynamic Range (Textbox)\n" +
                "           - range of byte, when convered from int16\n" +
                "           - Eq: byte=((Zmap + Offset) / DynamicRange) * 255\n" +
                "           - lower: smaller range(more sensitive to change)\n" +
                "                    more prone to overflow\n" +
                "           - larger: larger range(less sensitive to change)\n" +
                "                    less prone to overflow\n" +
                "       3.2 Zmap Offset (Textbox)\n" +
                "           - Eq: byte=((Zmap + Offset) / DynamicRange) * 255\n" + 
                "       3.3 Background Height (Textbox)\n" +
                "           - background height of object\n" +
                "       3.4 Auto calibration (Button) \n" +
                "           - automatically set tab 3.2, tab 3.3\n" +
                "           - reference taken from center point\n" +
                "       3.5 Offset Safety (Textbox)\n" +
                "           - margin of whats considered as background\n" +
                "   4. Save Image\n");
        }

        private void checkBox_MinAreaRect_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_MinAreaRect.Checked == true)
            {
                trackBar_RatioFilter.Enabled = true;
                checkBox_RGBAsZmap.Enabled = true;
                checkBox_AngleCorr.Enabled = true;
                textBox_MinPixelArea.Enabled = true;
                textBox_Rounding.Enabled = true;

                //ROI
                trackBar_ROIx.Enabled = true;
                trackBar_ROIy.Enabled = true;
                trackBar_ROIScale.Enabled = true;
            }
            else
            {
                trackBar_RatioFilter.Enabled = false;
                checkBox_RGBAsZmap.Enabled = false;
                checkBox_AngleCorr.Enabled = false;
                textBox_MinPixelArea.Enabled = false;
                textBox_Rounding.Enabled = false;

                //ROI
                trackBar_ROIx.Enabled = false;
                trackBar_ROIy.Enabled = false;
                trackBar_ROIScale.Enabled = false;
            }
        }

        //-------------------------------------------------------------------------------------------------------
        // Functions
        //-------------------------------------------------------------------------------------------------------
        
        //unused (for now)
        private byte[] ImageToByte(Bitmap img)
        {
            byte[] Stream = new byte[img.Width * img.Height * 3];
            int i = 0;
            int x = 0;
            int y = 0;

            Color color;
            for (y = 0; y < img.Height; y++)
            {
                for (x = 0; x < img.Width; x++)
                {
                    color = img.GetPixel(x, y);
                    Stream[i++] = color.R;
                    Stream[i++] = color.G;
                    Stream[i++] = color.B;
                }
            }
            return Stream;
        }


        /// <summary>
        /// Mixes RGB bitmap with Zmap
        /// </summary>
        private Bitmap mixedMap(byte[] RGB_arry, ushort[] Zmap,UInt16 scalingMaxValue)
        {
            Byte[] temp_arry = new byte[640 * 512 * 4];//RGB_arry;
            int i = 0;//4 in array

            for (int i2 = 0; i2 < Zmap.Length; i2++)
            {   
                    temp_arry[i + 2] = RGB_arry[i];
                    temp_arry[i + 1] = RGB_arry[i + 1];
                    temp_arry[i] = RGB_arry[i + 2];
                    temp_arry[i + 3] = (byte)(((Zmap[i2]+Zmap_Offset) / (double)scalingMaxValue) * byte.MaxValue); //replace A to ZMap
                    i = i + 4;
            }
            Bitmap resultMap = CopyDataToBitmap(temp_arry);
            return resultMap;
        }

        /// <summary>
        /// converts array to 32bppArgb bitmap
        /// </summary>
        public Bitmap CopyDataToBitmap(byte[] data)
        {
            //Here create the Bitmap to the know height, width and format
            Bitmap bmp = new Bitmap(640, 512, PixelFormat.Format32bppArgb);

            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(
                                 new Rectangle(0, 0, bmp.Width, bmp.Height),
                                 ImageLockMode.WriteOnly, bmp.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

            //Unlock the pixels
            bmp.UnlockBits(bmpData);

            //Return the bitmap 
            return bmp;
        }

        /// <summary>
        /// parses data coming from backend
        /// </summary>
        void parsedata(String data)
        {
            String[] _commandArry = data.Split(' ');
            for (int i=0; i<=_commandArry.Length; i=i+4)
            {
                drawBox(Convert.ToInt32(_commandArry[i]), Convert.ToInt32(_commandArry[i + 1]), Convert.ToInt32(_commandArry[i + 2]), Convert.ToInt32(_commandArry[i + 3]));
            }
        }

        void drawBox(int x, int y, int w, int h)
        {

            Graphics gF = pictureBox2.CreateGraphics();
            Pen Pen = new Pen(Color.FromArgb(255, 0, 255, 0), 5);
            gF.DrawRectangle(Pen, x, y, w, h);
        }
        public static PointF[] PointToPointF(Point[] pf)
        {
            PointF[] p = new PointF[pf.Length];
            int num = 0;
            foreach (var point in pf)
            {
                p[num].X = (int)point.X;
                p[num++].Y = (int)point.Y;
            }
            return p;
        }
        double DegToRad(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
        double RadToDeg(double Rad)
        {
            return Rad * 180 / Math.PI;
        }

        /// <summary>
        /// checks and sets the range of textboxes to prevent crashes
        /// </summary>
        void setTextboxRange()
        {
            //zmap dynamic range
            if (textBox_DynamicRange.Text == "")
            {
                textBox_DynamicRange.Text = "1";
            }
            else if (Convert.ToUInt32(textBox_DynamicRange.Text) > 50000)
            {
                textBox_DynamicRange.Text = "50000";
            }
            else
            {
                Zmap_DR = Convert.ToUInt16(textBox_DynamicRange.Text);
            }

            //zmap offset
            if (textBox_ZmapOffset.Text == "")
            {
                textBox_ZmapOffset.Text = "0";
            }
            else if (Convert.ToUInt32(textBox_ZmapOffset.Text) > 30000)
            {
                textBox_ZmapOffset.Text = "30000";
            }
            else
            {
                Zmap_Offset = Convert.ToUInt16(textBox_ZmapOffset.Text);
            }

            //background height
            if (textBox_BackgroundH.Text=="")
            {
                textBox_BackgroundH.Text = "1.5";
            }
            else if (Convert.ToDouble(textBox_BackgroundH.Text)>10)
            {
                textBox_BackgroundH.Text = "10";
            }
            else
            {
                BackgroundH = Convert.ToDouble(textBox_BackgroundH.Text);
            }

            //minimum pixel area filter
            if (textBox_MinPixelArea.Text == "")
            {
                textBox_MinPixelArea.Text = "0";
            }
            else if (Convert.ToUInt32(textBox_MinPixelArea.Text) > 5000)
            {
                textBox_MinPixelArea.Text = "10000";
            }
            else
            {
                MinPixelArea = Convert.ToUInt16(textBox_MinPixelArea.Text);
            }              
        }

        /// <summary>
        /// init save directories
        /// </summary>
        void CreateDirectory ()
        {
            System.IO.Directory.CreateDirectory(textBox_Savepath.Text + "/Output/rgb");
            System.IO.Directory.CreateDirectory(textBox_Savepath.Text + "/Output/dep");
            System.IO.Directory.CreateDirectory(textBox_Savepath.Text + "/Output/mixed");
            System.IO.Directory.CreateDirectory(textBox_Savepath.Text + "/Output/Detection Result");
            System.IO.Directory.CreateDirectory(textBox_Savepath.Text + "/Output/PointCloud");
        }
    }
}
