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
                    if (textBox_DynamicRange.Text == "")
                    {
                        textBox_DynamicRange.Text = "1";
                    }
                    bitmap = depthMap.ZMap.ToBitmap(Convert.ToUInt16(textBox_DynamicRange.Text));
                    this.label1.Text = bitmap.GetPixel(320, 250).R.ToString();

                    bitmap_RGB = depthMap.RgbaMap.ToBitmap();
                    bitmap_arry = depthMap.RgbaMap.Data.ToArray();
                    ZMap_arry = depthMap.ZMap.Data.ToArray();

                    if (checkBox_MinAreaRect.Checked == true)
                    {
                       

                        Image<Bgr, byte> a = new Image<Bgr, byte>(bitmap);
                        Image<Gray, byte> b = new Image<Gray, byte>(a.Width, a.Height);         //边缘检测
                        Image<Gray, byte> c = new Image<Gray, byte>(a.Width, a.Height);         //用于寻找轮廓 
                        Image<Bgr, byte> d = new Image<Bgr, byte>(a.Width, a.Height);           //用于绘制轮廓

                        //a.ROI =new Rectangle(x, y, w+offset, h+offset);

                        int cannytherhold = 100;
                        CvInvoke.Canny(a, b, cannytherhold / 2, cannytherhold);

                        VectorOfVectorOfPoint con = new VectorOfVectorOfPoint();
                        CvInvoke.FindContours(b, con, c, RetrType.List, ChainApproxMethod.ChainApproxNone);
                        double[] temp = new double[10000];
                        Point[][] con1 = con.ToArrayOfArray();
                        PointF[][] con2 = Array.ConvertAll<Point[], PointF[]>(con1, new Converter<Point[], PointF[]>(PointToPointF));
                        for (int i = 0; i < con.Size; i++)
                        {
                            temp[i] = CvInvoke.ContourArea(con[i],false);

                            if (CvInvoke.ContourArea(con[i],false) <= 100)
                            { continue; }

                            RotatedRect rrec = CvInvoke.MinAreaRect(con2[i]);       //g

                            PointF[] pointfs = rrec.GetVertices();
                           for (int j = 0; j < pointfs.Length; j++)
                                CvInvoke.Line(a, new Point((int)pointfs[j].X, (int)pointfs[j].Y), new Point((int)pointfs[(j + 1) % 4].X, (int)pointfs[(j + 1) % 4].Y), new MCvScalar(0, 255, 0, 255), 4);
                        }
                        for (int i = 0; i < con.Size; i++)
                        {
                            CvInvoke.DrawContours(d, con, i, new MCvScalar(255, 255, 0, 255), 2);
                        }
                            
                        //CvInvoke.Inpaint();
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

        private void button2_Click(object sender, EventArgs e)
        {
            drawBox(200, 200, 100, 100);
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
    }
}
