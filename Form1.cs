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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static String host = "192.168.1.10";
        static int port = 2114;
        VisionaryDataStream dataStream = new VisionaryDataStream(host, port);
        VisionaryControl control = new VisionaryControl(host);
        int index = 0;
        Bitmap bitmap_RGB;
        

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
                Byte[] bytes = new Byte[256];
                String data = null;

                try
                {
                    IPAddress ipAddress = Dns.Resolve("localhost").AddressList[0];
                    TcpListener server = new TcpListener(ipAddress, 12201);
                    System.Threading.Thread.Sleep(1000);
                    server.Start();
                    MessageBox.Show("Waiting for client to connect...");
                    TcpClient client = server.AcceptTcpClient();


                    NetworkStream nStream = client.GetStream();
                    

                    //network loop, send then wait for reply loop
                    while (true)
                    {
                        // The 'using' here will call Dispose on the client after data is sent.
                        // This will disconnect the client
                        //byte[] bStream = converterDemo(bitmap_RGB);
                        try
                            {
                            //transmit 1 frame
                            bitmap_RGB.Save("C:/Users/Koland Mak/Desktop/UON/placement/deep learning/sample/locate box/temp.png");
                            //nStream.Write(bStream, 0, bStream.Length);
                            //end of stream
                            String cmd = "END";
                                Byte[] endOfStream_ary = Encoding.UTF8.GetBytes(cmd);
                                Byte[] endOfStream = new Byte[cmd.Length + 2];
                                for (int i2 = 0; i2 < cmd.Length; i2++)
                                {
                                endOfStream[i2 + 1] = endOfStream_ary[i2];
                                }
                            endOfStream[cmd.Length + 1] = 0x0A;

                                nStream.Write(endOfStream, 0, endOfStream.Length);
                            }
                            catch (SocketException e1)
                            {
                                MessageBox.Show("SocketException: " + e1);
                            }

                        bool Replied = false;
                            int i;
                            while (((i = nStream.Read(bytes, 0, bytes.Length)) != 0)&&Replied==false)
                            {
                                // Translate data bytes to a ASCII string.
                                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                //MessageBox.Show("Received:" + data);

                                // Process the data sent by the client.
                                //data = data.ToUpper();

                                //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                                // Send back a response.
                                //stream.Write(msg, 0, msg.Length);
                                //MessageBox.Show("Sent" + data);
                                Replied = true;

                        }

                        System.Threading.Thread.Sleep(100);

                    }
                }
                catch (SocketException e1)
                {
                    MessageBox.Show("SocketException: " + e1);
                }   

            /*
                TcpListener server = null;
                try
                {
                    // Set the TcpListener on port 12201
                    Int32 port = 12201;
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                    // TcpListener server = new TcpListener(port);
                    server = new TcpListener(localAddr, port);

                    // Start listening for client requests.
                    server.Start();

                    // Buffer for reading data
                    Byte[] bytes = new Byte[256];
                    String data = null;

                    // Enter the listening loop.
                    while (true)
                    {
                        MessageBox.Show("Waiting for a connection... ");

                        // Perform a blocking call to accept requests.
                        // You could also user server.AcceptSocket() here.
                        TcpClient client = server.AcceptTcpClient();
                        MessageBox.Show("Connected!");

                        data = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i;

                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            MessageBox.Show("Received:"+ data);

                            // Process the data sent by the client.
                            data = data.ToUpper();

                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            MessageBox.Show("Sent"+ data);
                        }

                        // Shutdown and end connection
                        client.Close();
                    }
                }
                catch 
                {
                    MessageBox.Show("SocketException");
                }
                finally
                {
                    // Stop listening for new clients.
                    server.Stop();
                }
                */
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
                    Console.WriteLine($"Frame received through step called, frame #{depthMap.FrameNumber}, timestamp: {depthMap.TimestampMs}");
                    
                    //// Important: When converting multiple frames, make sure to re-use the same converter as it will result in much better performance.
                    PointCloudConverter converter = new PointCloudConverter();
                    Vector3[] pointCloud = converter.Convert(depthMap);
                    float z = pointCloud[250 * 640 + 320].Z;
                    this.label1.Text = z.ToString();
                    //MessageBox.Show("get fram");
                   
                 Bitmap bitmap = depthMap.ZMap.ToBitmap(8000);

                    this.pictureBox1.Image = bitmap;

                    bitmap_RGB = depthMap.RgbaMap.ToBitmap();
                    this.pictureBox2.Image = bitmap_RGB;

                }
            });
        }   

        private void button_Save_Click(object sender, EventArgs e)
        {
            index = Convert.ToInt32(textBox_Index.Text);
            bitmap_RGB.Save("C:/Users/Koland Mak/source/repos/Visionary S/WindowsFormsApp1/Output/" + index.ToString() + ".png");
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
        static byte[] ImageToByte(System.Drawing.Image iImage)
        {
            MemoryStream mMemoryStream = new MemoryStream();
            iImage.Save(mMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
            return mMemoryStream.ToArray();
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        public static byte[] converterDemo(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }


    }
}
