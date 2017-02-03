using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ImageClient
{
    /// <summary>
    /// Class for creating XML message
    /// </summary>
    public static class XmlCreator
    {

        #region "Methods"

        /// <summary>
        /// Create XML message
        /// </summary>
        /// <param name="grayscale">Greyscale - "1" yes, "0" no</param>
        /// <param name="flip">Flip - - "1" yes, "0" no</param>
        /// <param name="rotate">Rotate - degrees</param>
        /// <param name="imageBase64">base64 image string</param>
        /// <returns></returns>
        public static string MakeXMLMessage(string grayscale, string flip, string rotate, string imageBase64)
        {
            //make xml
            var sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                writer.WriteStartDocument(); //start document
                writer.WriteStartElement("Message"); //start element

                //grayscale
                writer.WriteStartElement("Grayscale");
                writer.WriteValue(grayscale);
                writer.WriteEndElement();

                //flip
                writer.WriteStartElement("Flip");
                writer.WriteValue(flip);
                writer.WriteEndElement();

                //rotate
                writer.WriteStartElement("Rotate");
                writer.WriteValue(rotate);
                writer.WriteEndElement();

                //image
                writer.WriteStartElement("Image");
                writer.WriteValue(imageBase64);
                writer.WriteEndElement();

                writer.WriteEndElement(); //end element
                writer.WriteEndDocument(); //end document
            }

            System.IO.File.WriteAllText("out_message.xml", sb.ToString()); //not necessary (for testing)
            return sb.ToString();

        }

        #endregion

    }
}
