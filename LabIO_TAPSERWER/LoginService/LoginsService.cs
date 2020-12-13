using ChatCommunication;
using Ciphering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace LoginService
{
    public static class LoginsService
    {
        private static string message;
        private static string login, pass;
        private static string FileName = "logins.txt";
        private static int ReceivedDataLength;
        private static int data_length = 1024;
        
        /// <summary>
        /// Funkcja sprawdzajaca istnienie pliku, oraz tworzaca go w razie jego braku
        /// </summary>
        /// <returns>true jesli plik istnieje, falsz jesli plik nie istnial</returns>
        public static bool CheckFile()
        {
            if (File.Exists(FileName))
            {
                return true;
            }
            else
            {
                File.Create(FileName).Dispose();
                return false;
            }
        }
        /// <summary>
        /// Funkcja Pytajaca uzytkownika o login oraz haslo
        /// </summary>
        /// <param name="stream">strumien klienta</param>
        /// <returns>string:(login,haslo)</returns>
        private static (string log, string pas) AskForLogin(NetworkStream stream)
        {
            byte[] buffer = new byte[data_length];
            //  message = "Podaj login: \r\n";
            // stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            do
            {
                ReceivedDataLength = stream.Read(buffer, 0, data_length);
            } while ((login = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength)) == "\r\n");


            //  message = "Podaj haslo: \r\n";

            //  stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            do
            {
                ReceivedDataLength = stream.Read(buffer, 0, data_length);
            } while ((pass = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength)) == "\r\n");

            return (login, pass);
        }

        /// <summary>
        /// Funkcja sprawdzajaca czy nazwa uzytkownika nie jest juz zajeta
        /// </summary>
        /// <param name="login">Login uzytkownika</param>
        /// <returns>True jesli nazwa zajeta i false jesli jest dostepna</returns>
        public static bool DoesUserExists(string login)
        {
            string temp;
            var streamReader = File.OpenText(FileName);
            while ((temp = streamReader.ReadLine()) != null)
            {
                temp = temp.Substring(0, temp.IndexOf(','));
                if (login == temp)
                {
                    streamReader.Close();
                    return true;
                }
            }
            streamReader.Close();
            return false;
        }

        /// <summary>
        /// Funkcja sprawdzajaca poprawnosc loginu i hasla w trakcie logowania
        /// </summary>
        /// <param name="login">Login uzytkownika</param>
        /// <param name="password">Haslo uzytkownika</param>
        /// <returns>True jesli dane logowania sa poprawne, false jesli sa bledne</returns>
        public static bool LoggingIn(string login, string password)
        {
            string temp;

            var streamReader = File.OpenText(FileName);
            while ((temp = streamReader.ReadLine()) != null)
            {
                string[] credentials = temp.Split(',');
                if (login == Cipher.Decrypt(credentials[0]) && password == Cipher.Decrypt(credentials[1]))
                {
                    streamReader.Close();
                    return true;
                }
            }
            streamReader.Close();
            return false;
        }
        /// <summary>
        /// Funkcja zajmuajca sie logowaniem uzytkownika, wysyla wiadomosci do uzytkownika oraz rozpatruje jego odpowiedz
        /// </summary>
        /// <param name="stream">strumien klienta</param>
        public static void LoginHandle(NetworkStream stream)
        {
            byte[] buffer = new byte[data_length];
            string login, haslo;
            string mes;
            int wybor = 1;
            (login, haslo) = AskForLogin(stream);
            if (LoggingIn(login, haslo) && !Communication.lista_zalogowanych.ContainsKey(login))
            {
                Communication.lista_zalogowanych.Add(login,stream);
                message = "1";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                Thread.Sleep(100);
                message = string.Join(",", Communication.lista_zalogowanych.Keys);
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);

                Communication.Chat_loop(stream,login);

                message = "udalosiealepocichu";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                Communication.lista_zalogowanych.Remove(login);  

                // message = "Udalo sie wylogowac zegnam\r\n\r\n";
                // stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                // message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
                // stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
            else if(Communication.lista_zalogowanych.ContainsKey(login))
            {
                message = "2";        
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                //message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
                // stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
            else
            {
                message = "0";         
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }

        }

        /// <summary>
        /// Funkcja zajmujaca sie rejestrowaniem uzytkownika, wysyla wiadomosci do uzyktownika oraz rozpatruje jego odpowiedz
        /// </summary>
        /// <param name="stream">strumien klienta</param>
        public static void RegisterHandle(NetworkStream stream)
        {
            byte[] buffer = new byte[data_length];
            string login, password;
            (login, password) = AskForLogin(stream);
            if (!DoesUserExists(Cipher.Encrpyt(login)))
            {
                var streamWriter = File.AppendText(FileName);
                login = Cipher.Encrpyt(login);
                password = Cipher.Encrpyt(password);
                streamWriter.WriteLine($"{login},{password}");
                streamWriter.Close();
                message = "1";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                //  message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
                // stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
            else
            {

                message = "0";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                //message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
                // stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
        }
    }
}
