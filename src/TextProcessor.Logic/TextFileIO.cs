using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TextProcessor.Logic
{
    public class TextFileIO
    {

        public string ImportFromFile(string filePath)
        {
            // Pre-условия
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Pre-условие нарушено: путь к файлу не может быть пустым");
            }

            if (Path.GetExtension(filePath).ToLower() != ".txt")
            {
                throw new ArgumentException("Pre-условие нарушено: поддерживаются только .txt файлы");
            }

            string text = File.ReadAllText(filePath, Encoding.UTF8);

            // Post-условия
            Debug.Assert(text != null, "Post-условие нарушено: результат импорта не может быть null");
            Debug.Assert(text.Length >= 0, "Post-условие нарушено: длина текста не может быть отрицательной");

            return text;
        }

        public string[] ImportFromDirectory(string directoryPath, string searchPattern = "*.txt")
        {
            // Pre-условия
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentException("Pre-условие нарушено: путь к директории не может быть пустым");
            }

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("Pre-условие нарушено: директория не найдена");
            }

            string[] files = Directory.GetFiles(directoryPath, searchPattern);
            string[] results = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                results[i] = ImportFromFile(files[i]);
            }

            // Post-условия
            Debug.Assert(results != null, "Post-условие нарушено: массив результатов не может быть null");
            Debug.Assert(results.Length == files.Length, "Post-условие нарушено: количество результатов не совпадает с количеством файлов");

            return results;
        }
        public void ExportToFile(string filePath, string text)
        {
            // Pre-условия
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Pre-условие нарушено: путь к файлу не может быть пустым");
            }

            if (Path.GetExtension(filePath).ToLower() != ".txt")
            {
                throw new ArgumentException("Pre-условие нарушено: поддерживаются только .txt файлы");
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text), "Pre-условие нарушено: текст не может быть null");
            }
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, text, Encoding.UTF8);

            // Post-условия
            Debug.Assert(File.Exists(filePath), "Post-условие нарушено: файл должен быть создан");

            string writtenText = File.ReadAllText(filePath, Encoding.UTF8);
            Debug.Assert(writtenText == text, "Post-условие нарушено: содержимое файла не совпадает с переданным текстом");
        }

        public void ExportToFile(string filePath, string[] lines)
        {
            // Pre-условия
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Pre-условие нарушено: путь к файлу не может быть пустым");
            }

            if (Path.GetExtension(filePath).ToLower() != ".txt")
            {
                throw new ArgumentException("Pre-условие нарушено: поддерживаются только .txt файлы");
            }

            if (lines == null)
            {
                throw new ArgumentNullException(nameof(lines), "Pre-условие нарушено: массив строк не может быть null");
            }

            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllLines(filePath, lines, Encoding.UTF8);

            // Post-условия
            Debug.Assert(File.Exists(filePath), "Post-условие нарушено: файл должен быть создан");
            string[] writtenLines = File.ReadAllLines(filePath, Encoding.UTF8);
            Debug.Assert(writtenLines.Length == lines.Length, "Post-условие нарушено: количество строк в файле не совпадает");
        }
    }
}