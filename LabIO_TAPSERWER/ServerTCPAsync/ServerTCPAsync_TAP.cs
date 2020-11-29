using System;
using ServerTCPLib;
using LoginService;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace ServerTCPAsync 
{
    public class ServerTCPAsync_TAP : ServerTCP
    {
        public ServerTCPAsync_TAP(IPAddress ip, int port) : base(ip, port)
        {
            _ip = ip;
            _port = port;
            LoginsService.CheckFile();
        }

        public override void AcceptClient()
        {
            while (true)
            {

                TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                _stream = tcpClient.GetStream();
                Task.Run(() => BeginDataTransmission(_stream));
            }
        }

        public override void Start()
        {
            StartListening();
            AcceptClient();
        }
        
        protected override void BeginDataTransmission(NetworkStream stream)
        {
            buffer = new byte[_data_length];

            string anwser;
             string message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
              message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            while (true)
            {

                while (stream.DataAvailable)
                {
                    ReceivedDataLength = stream.Read(buffer, 0, _data_length);
                    anwser = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength);

                    if (anwser[0] == '1')
                    {

                        LoginsService.LoginHandle(stream);
                    }
                    else if (anwser[0] == '2')
                    {
                        LoginsService.RegisterHandle(stream);
                    }
                }


            }
        }
    }
}
