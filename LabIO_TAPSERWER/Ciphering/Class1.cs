using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Ciphering
{
    public static class Cipher
    {

        private static readonly string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ_. ";
        private static readonly int alphabetLength = alphabet.Length;
        private static string key = "ugabuga";


        static int GetPosition(char letter)
        {
            return alphabet.IndexOf(char.ToUpper(letter));
        }

        static int Modulo(int a, int b)
        {
            return (a + b) % alphabetLength;
        }

        static public string Encrpyt(string text)
        {
            StringBuilder outputText = new StringBuilder(text);

            int keyIterator = 0;

            for (int i = 0; i < text.Length; i++)
            {
                var outputTextLetterPositionInAlphabet = GetPosition(outputText[i]);

                if (outputTextLetterPositionInAlphabet >= 0)
                {
                    var keyLetterPositionInAlphabet = GetPosition(key[keyIterator]);

                    var encryptedLetter = alphabet[Modulo(GetPosition(alphabet[outputTextLetterPositionInAlphabet]), GetPosition(alphabet[keyLetterPositionInAlphabet]))];

                    outputText[i] = char.IsUpper(outputText[i]) ? encryptedLetter : char.ToLower(encryptedLetter);
                }

                keyIterator = keyIterator + 1 == key.Length ? 0 : ++keyIterator;
            }

            return outputText.ToString();
        }

        static public string Decrypt(string encryptedText)
        {
            StringBuilder outputText = new StringBuilder(encryptedText);

            int keyIterator = 0;

            for (int i = 0; i < encryptedText.Length; i++)
            {
                var outputTextLetterPositionInAlphabet = GetPosition(outputText[i]);

                if (outputTextLetterPositionInAlphabet >= 0)
                {
                    var keyLetterPositionInAlphabet = GetPosition(key[keyIterator]);

                    char encryptedLetter;

                    if (outputTextLetterPositionInAlphabet >= keyLetterPositionInAlphabet)
                    {
                        encryptedLetter = alphabet[outputTextLetterPositionInAlphabet - keyLetterPositionInAlphabet];

                        outputText[i] = char.IsUpper(outputText[i]) ? encryptedLetter : char.ToLower(encryptedLetter);
                    }
                    else
                    {
                        encryptedLetter = alphabet[alphabetLength - Math.Abs(outputTextLetterPositionInAlphabet - keyLetterPositionInAlphabet) % alphabetLength];

                        outputText[i] = char.IsUpper(outputText[i]) ? encryptedLetter : char.ToLower(encryptedLetter);
                    }
                }

                keyIterator = keyIterator + 1 == key.Length ? 0 : ++keyIterator;
            }

            return outputText.ToString();
        }


    }
}