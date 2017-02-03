using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageServer
{

    /// <summary>
    /// Class for handling server communication with clients
    /// </summary>
    public class TcpServerHandler
    {

        #region "Data members" 

        private TcpListener _server;
        private Boolean _isRunning;

        #endregion

        #region "Methods"

        /// <summary>
        /// Initialize Tcp server handler with port
        /// </summary>
        /// <param name="port">Port for listening client requests</param>
        public TcpServerHandler(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start(); //start listening

            _isRunning = true;

            LoopClients();
        }

        /// <summary>
        /// Wait for client connection and process it in new thread
        /// </summary>
        private void LoopClients()
        {
            while (_isRunning)
            {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();

                //client connected
                //create a thread to handle communication
                Console.WriteLine("\nClient connected.");
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient); //start new thread
            }
        }

        /// <summary>
        /// Handle communication with client, receive message and send response
        /// </summary>
        /// <param name="obj">Tcp communication object</param>
        private void HandleClient(object obj)
        {
            Console.WriteLine("Data received from client. Proccesing request...");

            //retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;

            //sets two streams for reading and writing
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            StreamReader sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
            
            string sData = null;

            try
            {
                // reads client request (message) from stream
                sData = sReader.ReadLine();

                //process client request
                if (sData != null && sData != string.Empty)
                {
                    string imgBase64 = XmlParser.ReadKeyValue("Image", sData);
                 
                    //is image valid
                    if (imgBase64 != string.Empty)
                    {
                        Console.WriteLine("Image recieved.");

                        Image img = ImageEncoder.Base64ToImage(imgBase64);

                        //apply image filters on input image
                        Console.WriteLine("Applying image filters.");
                        Image imgResult = ApplyImageFilters(img, sData);
                        
                        //return result
                        Console.WriteLine("Returning image to client.");
                        sData = ImageEncoder.ImageToBase64(imgResult, System.Drawing.Imaging.ImageFormat.Jpeg); 
                    }
                    else
                    {
                        Console.WriteLine("Bad image recieved. Processing aborted.");
                    }
                }

                // to write something back.
                sWriter.WriteLine(sData);
                sWriter.Flush();
            }
            catch
            {}
            
        }

        /// <summary>
        /// Apply filters on image
        /// </summary>
        /// <param name="img">Image object</param>
        /// <param name="xmlData">XML string with processing information</param>
        /// <returns></returns>
        private Image ApplyImageFilters(Image img, string xmlData)
        {
            Image result = null;

            try
            {

                //apply filters
                string grayscaleChecked = XmlParser.ReadKeyValue("Grayscale", xmlData);
                string flipChecked = XmlParser.ReadKeyValue("Flip", xmlData);
                string rotateDegress = XmlParser.ReadKeyValue("Rotate", xmlData);

                if (grayscaleChecked == "1")
                {
                    img = ImageConverter.MakeGrayscale(img);
                    Console.WriteLine("Grayscale: YES");
                }
                else
                {
                    Console.WriteLine("Grayscale: NO");
                }

                if (flipChecked == "1")
                {
                    img = ImageConverter.Mirror(img);
                    Console.WriteLine("Flip: YES");
                }
                else
                {
                    Console.WriteLine("Flip: NO");
                }

                if (rotateDegress != "0")
                {
                    img = ImageConverter.Rotate(img, rotateDegress);
                    Console.WriteLine("Rotate: YES " + " (" + rotateDegress + ")");
                }
                else
                {
                    Console.WriteLine("Rotate: NO");
                }

                result = img;
            }
            catch
            { }

            return result;
        }

        #endregion

    }
}
