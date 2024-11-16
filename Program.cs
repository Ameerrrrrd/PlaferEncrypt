using System;
using System.Collections.Generic;
using System.Linq;

class PlayfairCipher
{
    // Статическая таблица для шифрования
    private static char[,] table = new char[5, 5];

    // Словарь для быстрого поиска позиции буквы в таблице
    private static Dictionary<char, (int, int)> positionDict = new Dictionary<char, (int, int)>();

    // Метод для генерации таблицы 5x5 из ключа
    private static void GenerateTable(string key)
    {
        // Убираем все символы, кроме букв, и переводим в верхний регистр
        key = new string(key.Where(c => char.IsLetter(c)).Select(c => char.ToUpper(c)).ToArray());

        // Объединяем ключ и алфавит (I и J считаем одинаковыми)
        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";  // Без J
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

    // Метод для разделения текста на пары букв
    private static List<string> PrepareText(string text)
    {
        // Убираем все символы, кроме букв
        text = new string(text.Where(c => char.IsLetter(c)).Select(c => char.ToUpper(c)).ToArray());

        // Заменяем J на I
        text = text.Replace('J', 'I');

        // Разделяем на пары
        List<string> pairs = new List<string>();
        for (int i = 0; i < text.Length; i += 2)
        {
            if (i + 1 < text.Length)
            {
                if (text[i] == text[i + 1]) // Если одинаковые буквы
                {
                    pairs.Add(text[i] + "X"); // Добавляем X как разделитель
                    i--;
                }
                else
                {
                    pairs.Add(text.Substring(i, 2));
                }
            }
            else
            {
                pairs.Add(text[i] + "X"); // Если нечётное количество букв, добавляем X в конец
            }
        }
        return pairs;
    }

    // Метод для шифрования пары букв
    private static string EncryptPair(string pair)
    {
        var (x1, y1) = positionDict[pair[0]];
        var (x2, y2) = positionDict[pair[1]];

        if (x1 == x2) // Буквы в одной строке
        {
            y1 = (y1 + 1) % 5;
            y2 = (y2 + 1) % 5;
        }
        else if (y1 == y2) // Буквы в одном столбце
        {
            x1 = (x1 + 1) % 5;
            x2 = (x2 + 1) % 5;
        }
        else // Буквы в разных строках и столбцах
        {
            (y1, y2) = (y2, y1); // Меняем столбцы
        }

        return $"{table[x1, y1]}{table[x2, y2]}";
    }

    // Метод для шифрования текста
    public static string Encrypt(string text, string key)
    {
        GenerateTable(key); // Генерация таблицы на основе ключа
        var pairs = PrepareText(text); // Разбиение текста на пары
        var encryptedText = string.Join("", pairs.Select(pair => EncryptPair(pair))); // Шифрование каждой пары
        return encryptedText;
    }

    static void Main(string[] args)
    {
        string key = "Khan"; // Ваш ключ
        string text = "I am Khan Ameer, and i'm autistic";  // Ваше сообщение

        string encryptedText = Encrypt(text, key);
        Console.WriteLine("Зашифрованный текст: " + encryptedText);
        if (encryptedText == "MKIAANBNSMDSNBCLSEQUMQQOEV")
            Console.WriteLine("true");
        Console.ReadKey();
    }
}
