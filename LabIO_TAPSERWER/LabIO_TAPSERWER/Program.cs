using System;
using System.Net;
using ServerTCPAsync;
namespace LabIO_TAPSERWER
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerTCPAsync_TAP server = new ServerTCPAsync_TAP(IPAddress.Parse("127.0.0.1"), 6000);
            server.Start();
            while (true) ;
        }
    }
}
