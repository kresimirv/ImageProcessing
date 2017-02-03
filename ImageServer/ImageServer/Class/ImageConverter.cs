using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ImageServer
{
    /// <summary>
    /// Class used for image manipulation operations
    /// </summary>
    public static class ImageConverter
    {

        #region "Methods"

        /// <summary>
        /// Returns grayscale Image
        /// </summary>
        /// <param name="img">Image object</param>
        /// <returns>Image object as grayscale</returns>
        public static Image MakeGrayscale(Image img)
        {
            Image result = null;

            Bitmap bm = new Bitmap(img);
            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {
                    Color c = bm.GetPixel(x, y);
                    int luma = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    bm.SetPixel(x, y, Color.FromArgb(luma, luma, luma));
                }

            }

            result = (Image)bm;
            return result;
        }

        /// <summary>
        /// Returns mirror image
        /// </summary>
        /// <param name="img">Image object</param>
        /// <returns>Image object mirrored</returns>
        public static Image Mirror(Image img)
        {
            Image result = null;

            img.RotateFlip(RotateFlipType.Rotate180FlipY);

            result = img;
            return result;
        }

        /// <summary>
        /// Returns rotated image by specified degrees
        /// </summary>
        /// <param name="img">Image object</param>
        /// <param name="degress">Degress to rotate</param>
        /// <returns>Image object rotated</returns>
        public static Image Rotate(Image img, string degress)
        {
            Image result = null;

            switch (degress)
            {
                case "90":
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case "180":
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case "270":
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }

            result = img;
            return result;
        }

        #endregion

    }
}
