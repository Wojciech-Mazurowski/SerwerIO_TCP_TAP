using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerTCPLib
{
    public abstract class ServerTCP
    {
       

        protected TcpListener _tcpListener;
        protected NetworkStream _stream;
        protected IPAddress _ip;
        protected int _port;
        protected int _data_length = 1024;
        protected byte[] buffer;
        protected int ReceivedDataLength;

        /// <summary>
        /// Konstruktor inicjalizujacy server
        /// </summary>
        /// <param name="ip">server ip</param>
        /// <param name="port">server port</param>
        public ServerTCP(IPAddress ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        /// <summary>
        /// Funckja pozwalajaca łączyć się klientom 
        /// </summary>
        public abstract void AcceptClient();
        /// <summary>
        /// Funkcja odpowiadajaca za pierwszą wymiane danych z użytkownikiem, pytająca czy użytkownik chce sie zalogowac czy zarejestrowac
        /// </summary>
        /// <param name="stream">strumien klienta</param>
        protected abstract void BeginDataTransmission(NetworkStream stream);

        protected void StartListening()
        {
            _tcpListener = new TcpListener(_ip, _port);
            _tcpListener.Start();

        }
     
        public abstract void Start();
    }
}
