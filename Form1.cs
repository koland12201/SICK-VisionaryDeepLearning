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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static String host = "192.168.0.164";
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
            try
            {

                Task.Run(async () =>
                {

                    if (!await dataStream.ConnectAsync())
                    {
                        //Debug.Print("shibai");
                        // Data stream connection failed
                        MessageBox.Show("error");
                    }
                    if (!await control.ConnectAsync())
                    {
                        // Data control (CoLaB) connection failed
                    }
                    //await control.StopAcquisitionAsync();

                    await control.StartAcquisitionAsync();





                });
            }
            catch (Exception ex)
            {

                throw;
            }


            Task.Run(async () =>
            {
                while (true)
                {
                    VisionaryFrame frame = await dataStream.GetNextFrameAsync();
                    //System.Threading.Thread.Sleep(100);
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
    }
}
