using System;
using System.Diagnostics;
using System.IO;
using System.Text;

public class TextFileIO
{
    // ИМПОРТ
    public string ImportFromFile(string filePath)
    {

        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("Путь к файлу не может быть пустым");
        }

        if (Path.GetExtension(filePath).ToLower() != ".txt")
        {
            throw new ArgumentException("Поддерживаются только .txt файлы");
        }

        string text = File.ReadAllText(filePath, Encoding.UTF8);

        Debug.Assert(text != null, "Результат импорта не может быть null");
        Debug.Assert(text.Length >= 0, "Длина текста не может быть отрицательной");

        return text;
    }

    public string[] ImportFromDirectory(string directoryPath, string searchPattern = "*.txt")
    {
        if (string.IsNullOrEmpty(directoryPath))
        {
            throw new ArgumentException("Путь к директории не может быть пустым");
        }

        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException("Директория не найдена");
        }

        string[] files = Directory.GetFiles(directoryPath, searchPattern);
        string[] results = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            results[i] = ImportFromFile(files[i]);
        }

        Debug.Assert(results != null, "Массив результатов не может быть null");
        Debug.Assert(results.Length == files.Length, "Количество результатов не совпадает с количеством файлов");

        return results;
    }
}
