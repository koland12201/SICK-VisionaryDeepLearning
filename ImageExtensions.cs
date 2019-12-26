using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Visionary.NET.Shared.Models;

namespace System.Drawing
{
    /// <summary>
    /// Extension methods for the <see cref="Image"/> subclasses.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Convert a UInt16IntensityImage to a Bitmap image.
        /// </summary>
        /// <param name="image">Image to convert.</param>
        /// <returns>Bitmap image.</returns>
        public static Bitmap ToBitmap(this UInt16IntensityImage image)
        {
            return image.ToBitmap(UInt16.MaxValue);
        }

        /// <summary>
        /// Convert a UInt16IntensityImage to a Bitmap image.
        /// </summary>
        /// <param name="image">Image to convert.</param>
        /// <returns>Bitmap image.</returns>
        public static Bitmap ToBitmap(this UInt16IntensityImage image, UInt16 scalingMaxValue)
        {
            Bitmap bitmap = new Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            byte[] imageData = new byte[bytesPerPixel * image.Width * image.Height];

            for (int y = 0; y < image.Height; y++)
            {
                int stereoOffset = y * image.Width;
                int imageOffset = y * image.Width * 3;
                for (int x = 0; x < image.Width; x++)
                {
                    // Scale from 16-bit to 8-bit
                    byte pixelValue = (byte)((image.Data[stereoOffset + x] / (double)scalingMaxValue) * byte.MaxValue);

                    // TODO: Scale to 8-bit instead of wrap
                    imageData[imageOffset + (x * bytesPerPixel)] = pixelValue;
                    imageData[imageOffset + (x * bytesPerPixel) + 1] = pixelValue;
                    imageData[imageOffset + (x * bytesPerPixel) + 2] = pixelValue;
                }
            }

            // Copy pixels into bitmap image
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            Marshal.Copy(imageData, 0, bitmapData.Scan0, imageData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// Convert a <see cref="UInt8RgbaImage"/> to a <see cref="Bitmap"/> image.
        /// </summary>
        /// <param name="image">Image to convert.</param>
        /// <returns>Bitmap image.</returns>
        public static Bitmap ToBitmap(this UInt8RgbaImage image)
        {
            // Create new bitmap image
            Bitmap bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            byte[] imageByteData = new byte[bytesPerPixel * image.Width * image.Height];

            int numberOfPixels = image.Width * image.Height;
            for (int i = 0; i < numberOfPixels; i++)
            {
                imageByteData[i * 3 + 0] = image.Data[i * 4 + 2];
                imageByteData[i * 3 + 1] = image.Data[i * 4 + 1];
                imageByteData[i * 3 + 2] = image.Data[i * 4 + 0];
            }

            // Copy pixels into bitmap image
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            Marshal.Copy(imageByteData, 0, bitmapData.Scan0, imageByteData.Length);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
