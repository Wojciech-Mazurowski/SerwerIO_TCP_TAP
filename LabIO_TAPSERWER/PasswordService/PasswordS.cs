using System;
using System.IO;
using System.Net.Sockets;
using Ciphering;



namespace PasswordService
{
    public class PasswordS
    {
        private static string FileName = "logins.txt";

        public static bool ChangePassword(string login, string new_password)
        {
            string temp;
            string[] arrLine = File.ReadAllLines(FileName);
            int i = 0;
            var streamReader = File.OpenText(FileName);
            while ((temp = streamReader.ReadLine()) != null)
            {
                temp = temp.Substring(0, temp.IndexOf(','));
                if (Cipher.Encrpyt(login) == temp)
                {
                    arrLine[i] = $"{Cipher.Encrpyt(login)},{Cipher.Encrpyt(new_password)}";
                    streamReader.Close();
                    File.WriteAllLines(FileName, arrLine);
                    return true;
                }
               
                i++;
            }
          
            return false;
        }
    }
}
