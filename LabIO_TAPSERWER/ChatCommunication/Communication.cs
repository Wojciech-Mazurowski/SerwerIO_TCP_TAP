using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatCommunication
{
    public class Communication
    {
        private static int ReceivedDataLength;
        private static int data_length = 1024;
        private static string message;
        public static Dictionary<string,NetworkStream> lista_zalogowanych;
        public static void Chat_loop(NetworkStream stream,string login)
        {
            string mes;
            NetworkStream temp;
            string[] split_mes;
            byte[] buffer = new byte[data_length];
            do
            {
                do
                {
                    ReceivedDataLength = stream.Read(buffer, 0, data_length);
                    mes = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength);
                } while ((mes = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength)) == "\r\n");
                if(mes == "%logout%")
                {
                    mes = "%logout%";
                }
                else if (mes == "%refresh%")
                {
                    message = "%lista%";
                    stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                    Thread.Sleep(100);
                    message = string.Join(",", Communication.lista_zalogowanych.Keys);
                    stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                }
                else
                {
                    split_mes = mes.Split('%');
                    temp = lista_zalogowanych[split_mes[0]];
                    message = login + "%" + split_mes[1];
                    temp.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                }

            } while(!((mes = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength)) == "%logout%"));

        }

    }
}
