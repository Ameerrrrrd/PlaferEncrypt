using System;
using System.Collections.Generic;
using System.Linq;

class PlayfairCipher
{
    private static char[,] table = new char[5, 5];

    private static Dictionary<char, (int, int)> positionDict = new Dictionary<char, (int, int)>();

    private static void GenerateTable(string key)
    {
        key = new string(key.Where(c => char.IsLetter(c)).Select(c => char.ToUpper(c)).ToArray());

        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";  
        string combinedKey = key + alphabet;

        // Убираем повторяющиеся символы
        combinedKey = string.Join("", combinedKey.Distinct());

        int index = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                table[i, j] = combinedKey[index];
                positionDict[table[i, j]] = (i, j);
                index++;
            }
        }
    }

    private static List<string> PrepareText(string text)
    {
        text = new string(text.Where(c => char.IsLetter(c)).Select(c => char.ToUpper(c)).ToArray());

        // Заменяем J на I
        text = text.Replace('J', 'I');

        List<string> pairs = new List<string>();
        for (int i = 0; i < text.Length; i += 2)
        {
            if (i + 1 < text.Length)
            {
                if (text[i] == text[i + 1])
                {
                    pairs.Add(text[i] + "X"); 
                    i--;
                }
                else
                {
                    pairs.Add(text.Substring(i, 2));
                }
            }
            else
            {
                pairs.Add(text[i] + "X"); 
            }
        }
        return pairs;
    }

    private static string EncryptPair(string pair)
    {
        var (x1, y1) = positionDict[pair[0]];
        var (x2, y2) = positionDict[pair[1]];

        if (x1 == x2) 
        {
            y1 = (y1 + 1) % 5;
            y2 = (y2 + 1) % 5;
        }
        else if (y1 == y2) 
        {
            x1 = (x1 + 1) % 5;
            x2 = (x2 + 1) % 5;
        }
        else 
        {
            (y1, y2) = (y2, y1);
        }

        return $"{table[x1, y1]}{table[x2, y2]}";
    }

    public static string Encrypt(string text, string key)
    {
        GenerateTable(key); 
        var pairs = PrepareText(text); 
        var encryptedText = string.Join("", pairs.Select(pair => EncryptPair(pair))); 
        return encryptedText;
    }

    static void Main(string[] args)
    {
        string key = "Khan";
        string text = "I am Khan Ameer, and i'm autistic";  

        string encryptedText = Encrypt(text, key);
        Console.WriteLine("Зашифрованный текст: " + encryptedText);
        if (encryptedText == "MKIAANBNSMDSNBCLSEQUMQQOEV")
            Console.WriteLine("true");
        Console.ReadKey();
    }
}
