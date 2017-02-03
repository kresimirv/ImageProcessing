using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace ImageServer
{
    /// <summary>
    /// Class for encoding and decoding images from/to base64 format
    /// </summary>
    public static class ImageEncoder
    {

        #region "Methods"

        /// <summary>
        /// Convert base 64 string representation of image to Image
        /// </summary>
        /// <param name="base64String">Base64 string</param>
        /// <returns>Image object</returns>
        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 string to byte[]
            Image ret = null;
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                ret = Image.FromStream(ms, true);

            }
            catch
            {  
            }

            return ret;
        }

        /// <summary>
        /// Convert Image object to base 64 string
        /// </summary>
        /// <param name="image">Image object</param>
        /// <param name="format">Image format</param>
        /// <returns>Base 64 string representation of Image</returns>
        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        #endregion

    }
}
