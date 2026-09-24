using System.Text.RegularExpressions;

namespace Elevator.Logic
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

            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            string result = text;

            result = Lowercase(result);

            result = RemovePunct(result);
            /// 3. Замена переносов строк и табуляций на пробелы
            /// 4. Удаление пробелов в начале и конце
            /// ...
            // Post-условия


            return result;
        }

        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ

        /// <summary>
        /// Приведение к нижнему регистру
        /// </summary>
        /// <param name="text">введённый текст</param>
        /// <returns>обработанный текст</returns>
        private string Lowercase(string text)
            { return text.ToLower(); }
        /// <summary>
        /// Удаление пунктуации и спецсимволов
        /// </summary>
        /// <param name="text">введённый текст</param>
        /// <returns>обработанный текст</returns>
        private string RemovePunct(string text)
        { return Regex.Replace(text, @"[^\w\s]", ""); }
    }
}
