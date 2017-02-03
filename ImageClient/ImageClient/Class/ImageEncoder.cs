using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace ImageClient
{
    /// <summary>
    /// Class for encoding and decoding images from/to base64 format and converting Image to BitmapImage
    /// </summary>
    public static class ImageEncoder
    {
        #region "Methods"

        /// <summary>
        /// Convert Base64 string representation of image to Image
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Image object</returns>
        public static Image Base64ToImage(string base64String)
        {
            //convert Base64 string to byte[]
            Image ret = null;
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                //convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                ret = Image.FromStream(ms, true);
            }
            catch
            { }

            return ret;
        }

        /// <summary>
        /// Convert Image object to Base 64 string
        /// </summary>
        /// <param name="image">Image object</param>
        /// <param name="format">Image format</param>
        /// <returns>Base 64 string representation of Image</returns>
        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                //convert byte[] to Base64 string
                string base64String = Convert.ToBase64String(imageBytes);
                
                return base64String;
            }
        }

        /// <summary>
        /// Convert Image object to BitmapImage
        /// </summary>
        /// <param name="image">Image object</param>
        /// <returns>BitmapImage object</returns>
        public static BitmapImage ToBitmapImage(this Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, image.RawFormat);
            ms.Seek(0, SeekOrigin.Begin);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        #endregion

    }
}
