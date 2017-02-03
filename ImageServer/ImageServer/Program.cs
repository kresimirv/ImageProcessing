using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Server started.");

                int port = Properties.Settings.Default.ServerPort; //get port setting
                TcpServerHandler server = new TcpServerHandler(port); //create tcp communication object
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error: {0}", e.StackTrace));
            }
        }
    }
}
