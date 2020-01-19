﻿using System;
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

        int index = 0;
        Bitmap bitmap;
        Bitmap bitmap_RGB;
        Bitmap bitmap_Mixed;
        byte[] bitmap_arry;
        ushort[] ZMap_arry;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //find contour
            Image<Bgr, byte> a = new Image<Bgr, byte>(@"C:\Users\Koland Mak\Desktop\UON\placement\deep learning\sample\locate box\mixed\dep\dep0.png");
            Image<Gray, byte> b = new Image<Gray, byte>(a.Width, a.Height);        //边缘检测
            Image<Gray, byte> c = new Image<Gray, byte>(a.Width, a.Height);         //用于寻找轮廓 
            Image<Bgr, byte> d = new Image<Bgr, byte>(a.Width, a.Height);           //用于绘制轮廓
            CvInvoke.Canny(a, b, 100, 60);
            VectorOfVectorOfPoint con = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(b, con, c, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);

            Point[][] con1 = con.ToArrayOfArray();
            PointF[][] con2 = Array.ConvertAll<Point[], PointF[]>(con1, new Converter<Point[], PointF[]>(PointToPointF));
            for (int i = 0; i < con.Size; i++)
            {
                Rectangle rec = CvInvoke.BoundingRectangle(con[i]);    //红色

                CircleF cir = CvInvoke.MinEnclosingCircle(con2[i]);     //蓝色

                RotatedRect rrec = CvInvoke.MinAreaRect(con2[i]);       //绿色

                PointF[] pointfs = rrec.GetVertices();
                for (int j = 0; j < pointfs.Length; j++)
                    CvInvoke.Line(a, new Point((int)pointfs[j].X, (int)pointfs[j].Y), new Point((int)pointfs[(j + 1) % 4].X, (int)pointfs[(j + 1) % 4].Y), new MCvScalar(0, 255, 0, 255), 4);

                CvInvoke.Rectangle(a, rec, new MCvScalar(0, 0, 255, 255), 4);
                CvInvoke.Circle(a, new Point((int)cir.Center.X, (int)cir.Center.Y), (int)cir.Radius, new MCvScalar(255, 0, 0, 255), 4);
            }
            for (int i = 0; i < con.Size; i++)
                CvInvoke.DrawContours(d, con, i, new MCvScalar(255, 0, 255, 255), 2);

            this.pictureBox2.Image = a.ConcateVertical(d).ToBitmap();
        }
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
                    TcpListener server = new TcpListener(ipAddress, 12201);
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

                        if (i > 0)
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
                            else if (data != "EMPTY\n")
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

                throw;
            }
            System.Threading.Thread.Sleep(1500);
            Task.Run(async () =>
            {
                while (true)
                {

                    VisionaryFrame frame = await dataStream.GetNextFrameAsync();

                    //System.Threading.Thread.Sleep(1000);
                    VisionarySDepthMapData depthMap = frame.GetData<VisionarySDepthMapData>();

                    //// Important: When converting multiple frames, make sure to re-use the same converter as it will result in much better performance.
                    PointCloudConverter converter = new PointCloudConverter();
                    Vector3[] pointCloud = converter.Convert(depthMap);
                    float z = pointCloud[250 * 640 + 320].Z;
                    // this.label1.Text = z.ToString();

                    bitmap = depthMap.ZMap.ToBitmap(25000);
                    this.label1.Text = bitmap.GetPixel(320, 250).R.ToString();
                    this.pictureBox1.Image = bitmap;

                    bitmap_RGB = depthMap.RgbaMap.ToBitmap();
                    bitmap_arry = depthMap.RgbaMap.Data.ToArray();
                    ZMap_arry = depthMap.ZMap.Data.ToArray();

                    this.pictureBox2.Image = bitmap_RGB;
                    try
                    {
                        bitmap_Mixed = mixedMap(bitmap_arry, ZMap_arry, 25000);
                        this.pictureBox_Mixed.Image = bitmap_Mixed;
                    }
                    catch { }
                }
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            drawBox(200, 200, 100, 100, 45);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap_RGB.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/rgb/rgb" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }

        private void button_Save_depth_Click(object sender, EventArgs e)
        {
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/dep/dep" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }

        private void button_Save_mixed_Click(object sender, EventArgs e)
        {
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap_Mixed.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/mixed/mixed" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }

        private void button_Save_all_Click(object sender, EventArgs e)
        {
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap_RGB.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/rgb/rgb" + index.ToString() + ".png");
            bitmap.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/dep/dep" + index.ToString() + ".png");
            bitmap_Mixed.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/mixed/mixed" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }

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

        private Bitmap mixedMap(byte[] RGB_arry, ushort[] Zmap,UInt16 scalingMaxValue)
        {
            Byte[] temp_arry = new byte[640 * 512 * 4];//RGB_arry;
            int i = 0;//4 in array
            for (int i2 = 0; i2 < Zmap.Length; i2++)
            {   
                    temp_arry[i + 2] = RGB_arry[i];
                    temp_arry[i + 1] = RGB_arry[i + 1];
                    temp_arry[i] = RGB_arry[i + 2];
                    temp_arry[i + 3] = (byte)((Zmap[i2] / (double)scalingMaxValue) * byte.MaxValue); //replace A to ZMap
                    i = i + 4;
            }
            Bitmap resultMap = CopyDataToBitmap(temp_arry);
            return resultMap;
        }

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
        void parsedata(String data)
        {
            String[] _commandArry = data.Split(' ');
            for (int i=0; i<=_commandArry.Length; i=i+4)
            {
                drawBox(Convert.ToInt32(_commandArry[i]), Convert.ToInt32(_commandArry[i + 1]), Convert.ToInt32(_commandArry[i + 2]), Convert.ToInt32(_commandArry[i + 3]),0);
            }
        }

        void drawBox(int x, int y, int w, int h,float deg)
        {
            //find contour
            Image<Bgr, byte> a = new Image<Bgr, byte>(@"C:\Users\Koland Mak\Desktop\UON\placement\deep learning\sample\locate box\jpg\1.png");
            Image<Gray, byte> b = new Image<Gray, byte>(a.Width, a.Height);        //边缘检测
            Image<Gray, byte> c = new Image<Gray, byte>(a.Width, a.Height);         //用于寻找轮廓 
            Image<Bgr, byte> d = new Image<Bgr, byte>(a.Width, a.Height);           //用于绘制轮廓

            VectorOfVectorOfPoint con = new VectorOfVectorOfPoint();

            Point[][] con1 = con.ToArrayOfArray();
            PointF[][] con2 = Array.ConvertAll<Point[], PointF[]>(con1, new Converter<Point[], PointF[]>(PointToPointF));

            for (int i = 0; i < con.Size; i++)
            {
                Rectangle rec = CvInvoke.BoundingRectangle(con[i]);    //红色

                CircleF cir = CvInvoke.MinEnclosingCircle(con2[i]);     //蓝色

                RotatedRect rrec = CvInvoke.MinAreaRect(con2[i]);       //绿色

                PointF[] pointfs = rrec.GetVertices();
                for (int j = 0; j < pointfs.Length; j++)
                    CvInvoke.Line(a, new Point((int)pointfs[j].X, (int)pointfs[j].Y), new Point((int)pointfs[(j + 1) % 4].X, (int)pointfs[(j + 1) % 4].Y), new MCvScalar(0, 255, 0, 255), 4);

                CvInvoke.Rectangle(a, rec, new MCvScalar(0, 0, 255, 255), 4);
                CvInvoke.Circle(a, new Point((int)cir.Center.X, (int)cir.Center.Y), (int)cir.Radius, new MCvScalar(255, 0, 0, 255), 4);
            }
            for (int i = 0; i < con.Size; i++)
                CvInvoke.DrawContours(d, con, i, new MCvScalar(255, 0, 255, 255), 2);

            this.pictureBox2.Image = a.ConcateVertical(d).ToBitmap();

            Graphics gF = pictureBox2.CreateGraphics();
            gF.RotateTransform(deg);
            Pen Pen = new Pen(Color.FromArgb(255, 0, 255, 0), 5);
            gF.DrawRectangle(Pen, x, y, w, h);
            
        }

        public static PointF[] PointToPointF(Point[] pf)
        {
            PointF[] aaa = new PointF[pf.Length];
            int num = 0;
            foreach (var point in pf)
            {
                aaa[num].X = (int)point.X;
                aaa[num++].Y = (int)point.Y;
            }
            return aaa;
        }
    }
}
