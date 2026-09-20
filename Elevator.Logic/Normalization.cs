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

            ///Методы для обработки теста:        
            /// 1. Приведение к нижнему регистру
            /// 2. Удаление пунктуации и спецсимволов
            /// 3. Замена переносов строк и табуляций на пробелы
            /// 4. Удаление пробелов в начале и конце
            /// ...
            // Post-условия


            return result;
        }
    }
}
