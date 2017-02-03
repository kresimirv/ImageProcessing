using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;


namespace ImageServer
{
    /// <summary>
    /// Class for reading values from XML document
    /// </summary>
    public static class XmlParser
    {
        #region "Methods"

        /// <summary>
        /// Read key value from XML document
        /// </summary>
        /// <param name="key">Key to read</param>
        /// <param name="xmlString">XML document as string</param>
        /// <returns>Key value</returns>
        public static string ReadKeyValue(string key, string xmlString)
        {
            string result = string.Empty;
            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlString));
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == key))
                {
                    result = xmlReader.ReadString();
                }
            }

            return result;
        }

        #endregion

    }
}
