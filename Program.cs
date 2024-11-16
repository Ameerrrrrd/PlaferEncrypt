using System;
using System.Linq;
using System.Text;

class PlayfairCipher
{
    private static string CreatePlayfairSquare(string key)
    {
        string alphabet = "АБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЩЫЭЮЯ";  
        StringBuilder square = new StringBuilder();
        key = key.ToUpper().Replace("Ё", "Е");  

        //  повторяющиеся символы
        foreach (char c in key)
        {
            if (!square.ToString().Contains(c.ToString()) && alphabet.Contains(c))
                square.Append(c);
        }

        foreach (char c in alphabet)
        {
            if (!square.ToString().Contains(c.ToString()))
                square.Append(c);
        }

        return square.ToString();
    }

    private static string PrepareText(string text)
    {
        text = text.ToUpper().Replace("Ё", "Е");  
        StringBuilder preparedText = new StringBuilder();

        foreach (char c in text)
        {
            if (char.IsLetter(c) || ",.?!".Contains(c))
            {
                preparedText.Append(c);
            }
        }

        for (int i = 0; i < preparedText.Length; i++)
        {

            if (i + 1 < preparedText.Length && preparedText[i] == preparedText[i + 1])
            {
                preparedText.Insert(i + 1, 'Ь'); 
                i++;
            }
        }


        if (preparedText.Length % 2 != 0)
        {
            preparedText.Append('Ь');
        }

        return preparedText.ToString();
    }

    private static string EncryptPair(char a, char b, string square)
    {
        int rowA = square.IndexOf(a) / 5;
        int colA = square.IndexOf(a) % 5;
        int rowB = square.IndexOf(b) / 5;
        int colB = square.IndexOf(b) % 5;


        if (rowA == rowB)
        {
            colA = (colA + 1) % 5;
            colB = (colB + 1) % 5;
        }
        else if (colA == colB) 
        {
            rowA = (rowA + 1) % 7;
            rowB = (rowB + 1) % 7;
        }
        else
        {
            int temp = colA;
            colA = colB;
            colB = temp;
        }

        return square[rowA * 5 + colA].ToString() + square[rowB * 5 + colB].ToString();
    }

    public static string Encrypt(string plaintext, string key)
    {
        string square = CreatePlayfairSquare(key);
        string preparedText = PrepareText(plaintext);
        StringBuilder ciphertext = new StringBuilder();

        // Проходим по парам символов и шифруем их
        for (int i = 0; i < preparedText.Length; i += 2)
        {
            char a = preparedText[i];
            char b = preparedText[i + 1];
            ciphertext.Append(EncryptPair(a, b, square));
        }

        return ciphertext.ToString();
    }

    public static string RestorePunctuation(string originalText, string encryptedText)
    {
        StringBuilder result = new StringBuilder();
        int encIndex = 0;

        for (int i = 0; i < originalText.Length; i++)
        {
            char c = originalText[i];

            if (char.IsLetter(c)) 
            {
                result.Append(encryptedText[encIndex]);
                encIndex++;
            }
            else if (",.?!".Contains(c))  
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    public static void Main()
    {
        Console.Write("Введите ключ: ");
        string key = Console.ReadLine();

        Console.Write("Введите текст для шифрования (с запятыми, точками и пробелами): ");
        string plaintext = Console.ReadLine();

        string ciphertext = Encrypt(plaintext, key);
        string resultText = RestorePunctuation(plaintext, ciphertext);

        Console.WriteLine($"Зашифрованный текст: {resultText}");
    
    
    
        Console.ReadKey();
    }
}
