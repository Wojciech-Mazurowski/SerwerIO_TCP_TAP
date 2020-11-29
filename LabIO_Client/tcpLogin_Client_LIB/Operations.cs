using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace tcpLogin_Client_LIB
{
    public class Operations
    {
        public static StringBuilder Login(NetworkStream stream, string username, string password)
        {
            string dane = "1";
            Client.WriteToStream(stream, dane);
            Thread.Sleep(100);
            Client.WriteToStream(stream, username);
            Thread.Sleep(100);
            Client.WriteToStream(stream, password);
            return Client.ReadFromStream(stream);
            

        }


        public static StringBuilder Register(NetworkStream stream, string username, string password)
        {
            string dane = "2";
            Client.WriteToStream(stream, dane);
            Thread.Sleep(100);
            Client.WriteToStream(stream, username);
            Thread.Sleep(100);
            Client.WriteToStream(stream, password);
            return Client.ReadFromStream(stream);
        }

        public static void Logout (NetworkStream stream)
        {
            string dane = "1";
            Client.WriteToStream(stream, dane);
        }
    }
}
