using System;
using System.Text.RegularExpressions;

namespace TextProcessor.Logic
{
    public class Normalization
    {
        // Основной метод
        public string Normalize(string text)
        {
            // Pre-условия
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text), "Pre-условие нарушено: текст не может быть null");
            }

            string result = text;

            result = Lowercase(result);
            result = RemoveSpecialChar(result);
            result = RemovePunct(result);
            result = CollapseWhitespace(result);
            result = TrimWhitespace(result);

            // Post-условия
            if (result != result.ToLower())
            {
                throw new InvalidOperationException("Post-условие нарушено: результат содержит символы верхнего регистра");
            }

            if (result.Contains("  "))
            {
                throw new InvalidOperationException("Post-условие нарушено: результат содержит двойные пробелы");
            }

            if (Regex.IsMatch(result, @"[^\w\s]"))
            {
                throw new InvalidOperationException("Post-условие нарушено: результат содержит знаки препинания");
            }

            return result;
        }

        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ

        /// <summary>
        /// Приведение к нижнему регистру
        /// </summary>
        /// <param name="text">введённый текст</param>
        /// <returns>обработанный текст</returns>
        private static string Lowercase(string text)
        {
            return text.ToLower();
        }

        /// <summary>
        /// Удаление пунктуации и спецсимволов
        /// </summary>
        /// <param name="text">введённый текст</param>
        /// <returns>обработанный текст</returns>
        private static string RemovePunct(string text)
        {
            return Regex.Replace(text, @"[^\w\s]", "");
        }

        /// <summary>
        /// Замена переносов строк и табуляций на пробелы
        /// </summary>
        /// <param name="text">введённый текст</param>
        /// <returns>обработанный текст</returns>
        private static string RemoveSpecialChar(string text)
        {
            return Regex.Replace(text, @"[\t\n\r]", " ");
        }

        /// <summary>
        /// Замена 2+ пробелов подряд на один
        /// </summary>
        /// <param name="text">введённый текст</param>
        /// <returns>обработанный текст</returns>
        private static string CollapseWhitespace(string text)
        {
            return Regex.Replace(text, @"\s+", " ");
        }

        /// <summary>
        /// Удаление пробелов в начале и конце
        /// </summary>
        /// <param name="text">введённый текст</param>
        /// <returns>обработанный текст</returns>
        private static string TrimWhitespace(string text)
        {
            return text.Trim();
        }
    }
}