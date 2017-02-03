using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageClient
{
    /// <summary>
    /// Server communication class
    /// </summary>
    public class TcpClientHandler
    {
        #region "Data members"

        private TcpClient _client;
        private StreamReader _sReader;
        private StreamWriter _sWriter;
        private bool _isConnected;

        private string _ipAddress;
        private int _port;

        #endregion

        #region "Methods"

        /// <summary>
        /// Is connection active
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        /// <summary>
        /// Create tcp client object (used for communication with server)
        /// </summary>
        /// <param name="ipAddress">Server IP address</param>
        /// <param name="portNum">Server port</param>
        public TcpClientHandler(string ipAddress, int portNum)
        {
           _ipAddress = ipAddress;
           _port = portNum;     
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        public void Connect()
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(_ipAddress, _port); //establish tcp connection
                _isConnected = true;
            }
            catch
            {
                _isConnected = false;
            }
        }

        /// <summary>
        /// Disconnect from server (close connection and dispose object)
        /// </summary>
        public void Disconnect()
        {
            //if connected
            if (_isConnected == true)
            {
                _client.Close();
            }
        }

        /// <summary>
        /// Send message to server and receive message response
        /// </summary>
        /// <param name="message">String to send</param>
        /// <returns>Response as string</returns>
        public string SendMessageAndReceive(string message)
        {
            string result = string.Empty;

            //if no message
            if (message == null || message == string.Empty)
            {
                return result;
            }

            //create streams
            _sReader = new StreamReader(_client.GetStream(), Encoding.ASCII);
            _sWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);
           
            //write data and flush buffer
            _sWriter.WriteLine(message);
            _sWriter.Flush();

            //get server response
            string sDataIncomming = _sReader.ReadLine();
            if (sDataIncomming != null)
            {
                result = sDataIncomming;
            }

            return result;

        }

        #endregion

    }
}
