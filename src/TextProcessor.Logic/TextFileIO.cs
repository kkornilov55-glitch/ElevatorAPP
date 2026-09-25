using System;
using System.IO;
using System.Text;

namespace TextProcessor.Logic
{
    public class TextFileIO
    {
        public string ImportFromFile(string filePath)
        {
            string text = File.ReadAllText(filePath, Encoding.UTF8);
            return text;
        }

        public string[] ImportFromDirectory(string directoryPath, string searchPattern = "*.txt")
        {
            string[] files = Directory.GetFiles(directoryPath, searchPattern);
            string[] results = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                results[i] = ImportFromFile(files[i]);
            }

            return results;
        }

        public void ExportToFile(string filePath, string text)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, text, Encoding.UTF8);
        }

        public void ExportToFile(string filePath, string[] lines)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllLines(filePath, lines, Encoding.UTF8);
        }
    }
}