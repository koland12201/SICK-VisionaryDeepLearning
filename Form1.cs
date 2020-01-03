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


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static String host = "192.168.1.10";
        static int port = 2114;
        VisionaryDataStream dataStream = new VisionaryDataStream(host, port);
        VisionaryControl control = new VisionaryControl(host);
        int index = 0;
        Bitmap bitmap;
        Bitmap bitmap_RGB;
        byte[] bitmap_arry;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                                /*
                                //Send back a response.
                                String cmd = "END";
                                Byte[] endOfStream_ary = Encoding.UTF8.GetBytes(cmd);
                                Byte[] endOfStream = new Byte[cmd.Length + 2];
                                for (int i2 = 0; i2 < cmd.Length; i2++)
                                {
                                    endOfStream[i2 + 1] = endOfStream_ary[i2];
                                }
                                endOfStream =new Byte[1];
                                nStream.Write(endOfStream, 0, endOfStream.Length);
                                */

                                //MessageBox.Show("Sent" + data);
                            }
                            else
                            {
                                
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
                    drawBox(10, 10, 100, 100);
                   
                    //System.Threading.Thread.Sleep(1000);
                    VisionarySDepthMapData depthMap = frame.GetData<VisionarySDepthMapData>();

                    //// Important: When converting multiple frames, make sure to re-use the same converter as it will result in much better performance.
                    PointCloudConverter converter = new PointCloudConverter();
                    Vector3[] pointCloud = converter.Convert(depthMap);
                    float z = pointCloud[250 * 640 + 320].Z;
                  // this.label1.Text = z.ToString();

                    bitmap = depthMap.ZMap.ToBitmap(20000);
                    this.label1.Text = bitmap.GetPixel(320, 250).R.ToString();
                    this.pictureBox1.Image = bitmap;

                    bitmap_RGB = depthMap.RgbaMap.ToBitmap();
                    bitmap_arry = depthMap.RgbaMap.Data.ToArray();
                    

                    this.pictureBox2.Image = bitmap_RGB;
                    try
                    {
                        Bitmap temp = mixedMap(bitmap_arry, bitmap);
                        this.pictureBox_Mixed.Image = temp;
                    }
                    catch { }
                    
                }
            });
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap_RGB.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/rbg" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }

        void drawBox(int x, int y, int w, int h)
        {
            Graphics gF = pictureBox2.CreateGraphics();
            gF.DrawRectangle(Pens.Red, x, y, w, h);

        }
        private void button2_Click(object sender, EventArgs e)
        {
            drawBox(10, 10, 100, 100);
        }

        private byte[] ImageToByte(Bitmap img)
        {
            byte[] Stream=new byte[img.Width* img.Height*3];
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

        private Bitmap mixedMap(byte[] RGB_arry, Bitmap Dmap)
        {
            Byte[] temp_arry = new byte[640 * 512 * 4];//RGB_arry;

            int i = 0;//4 in array
            for (int y = 0; y < Dmap.Height; y++)
            {
                for (int x = 0; x < Dmap.Width; x++)
                {
                    temp_arry[i + 2] = RGB_arry[i];
                    temp_arry[i+1] = RGB_arry[i + 1];
                    temp_arry[i] = RGB_arry[i + 2];
                    temp_arry[i + 3] =  Dmap.GetPixel(x, y).R; //Dmap A
                    i = i + 4;
                }
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
        private void button_Save_depth_Click(object sender, EventArgs e)
        {
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/dep" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }

        private void button_Save_mixed_Click(object sender, EventArgs e)
        {
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/mixed" + index.ToString() + ".png");
            index++;
            textBox_Index.Text = index.ToString();
        }
    }
}
