using Ciphering;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace LoginService
{
    public static class LoginsService
    {
        private static string message;
        private static string login, pass;
        private static string FileName = "logins.txt";
        private static int ReceivedDataLength;
        private static int _data_length = 1024;
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
            byte[] buffer = new byte[_data_length];
            message = "Podaj login: \r\n";
            stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            do
            {
                ReceivedDataLength = stream.Read(buffer, 0, _data_length);
            } while ((login = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength)) == "\r\n");


            message = "Podaj haslo: \r\n";

            stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            do
            {
                ReceivedDataLength = stream.Read(buffer, 0, _data_length);
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
            using var streamReader = File.OpenText(FileName);
            while ((temp = streamReader.ReadLine()) != null)
            {
                temp = temp.Substring(0, temp.IndexOf(','));
                if (login == temp)
                {
                    return true;
                }
            }
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

            using var streamReader = File.OpenText(FileName);
            while ((temp = streamReader.ReadLine()) != null)
            {
                string[] credentials = temp.Split(',');
                if (login == Cipher.Decrypt(credentials[0]) && password == Cipher.Decrypt(credentials[1]))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Funkcja zajmuajca sie logowaniem uzytkownika, wysyla wiadomosci do uzytkownika oraz rozpatruje jego odpowiedz
        /// </summary>
        /// <param name="stream">strumien klienta</param>
        public static void LoginHandle(NetworkStream stream)
        {
            byte[] buffer = new byte[_data_length];
            string login, haslo;
            string mes;
            int wybor = 1;
            (login, haslo) = AskForLogin(stream);
            if (LoggingIn(login, haslo))
            {

                message = "Udalo sie zalogowac, co chcesz zrobic?\r\n1.Nic\r\n2.Wylogujsie\r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                do
                {
                    ReceivedDataLength = stream.Read(buffer, 0, _data_length);
                } while ((mes = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength)) == "\r\n" || (mes = Encoding.ASCII.GetString(buffer, 0, ReceivedDataLength)) == "1");


                message = "Udalo sie wylogowac zegnam\r\n\r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
            else
            {
                message = "Nie udalo sie zalogowac :(\r\n\r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }

        }

        /// <summary>
        /// Funkcja zajmujaca sie rejestrowaniem uzytkownika, wysyla wiadomosci do uzyktownika oraz rozpatruje jego odpowiedz
        /// </summary>
        /// <param name="stream">strumien klienta</param>
        public static void RegisterHandle(NetworkStream stream)
        {
            byte[] buffer = new byte[_data_length];
            string login, password;
            (login, password) = AskForLogin(stream);
            if (!DoesUserExists(Cipher.Encrpyt(login)))
            {
                using var streamWriter = File.AppendText(FileName);
                login = Cipher.Encrpyt(login);
                password = Cipher.Encrpyt(password);
                streamWriter.WriteLine($"{login},{password}");
                streamWriter.Close();
                message = "Udalo sie zarejestrowac \r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
            else
            {

                message = "Uzytkownik juz istnieje / wybierz inna nazwe \r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                message = "1. Zalguj sie\r\n2. Zarejestruj sie\r\n";
                stream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            }
        }

    }
}