#region About
//
// Copyright note: Redistribution and use in source, with or without modification, are permitted.
// 
// Created: 2016-04-21
// 
// @author:	Johan Falk
// SICK AG, Waldkirch
// email: TechSupport0905@sick.de
// 
// Last commit: $Date: 2017-03-21 15:00:00 +0100 (tis, 21 mar 2017) $
// Last editor: $Author: richean $
// 
// Version "$Revision: 11783 $"
//
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace System.Drawing
{
    /// <summary>
    /// Extensions to System.Drawing.Bitmap class.
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// Converts this Bitmap to a BitmapImage.
        /// </summary>
        /// <param name="src">A Bitmap image</param>
        /// <returns>The image as a BitmapImage</returns>
        public static BitmapImage ToBitmapImage(this Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
