using System.Text;
using System.Text.RegularExpressions;
namespace Elevator.Logic
{
    /// <summary>
    /// Класс для обработки текста и приведения его к нужным форматам (маскам).
    /// </summary>
    public class TextMasking
    {
        /// <summary>
        /// Превращает сырую строку в российский номер формата +7 (XXX) XXX-XX-XX.
        /// Автоматически собирает статистику успехов и ошибок для UI.
        /// </summary>
        /// <param name="input">Входная строка от пользователя (может содержать мусор, скобки или быть пустой).</param>
        /// <returns>Объект ProcessResult, содержащий итоговый текст и списки пройденных/проваленных условий.</returns>
        public ProcessResult RuNumberMask(string input)
        {
            var pr = new ProcessResult();
            string rawInput = input ?? string.Empty;

            // --- Проверка Pre-условий ---
            const string isNotEmptyCheck = "Строка содержит данные";
            const string digitCountCheck = "Корректное количество цифр";

            if (string.IsNullOrWhiteSpace(rawInput))
            {
                pr.AddPre(isNotEmptyCheck, false);
                pr.isSuccess = false;
                return pr; //Прерываем выполнение, дальше идти нет смысла
            }
            pr.AddPre(isNotEmptyCheck, true);
            string normalizedNumber = NormalizeNumber(rawInput);

            if (normalizedNumber.Length != 10)
            {
                pr.AddPre(digitCountCheck, false);
                pr.isSuccess = false;
                return pr;
            }
            pr.AddPre(digitCountCheck, true);


            // --- Шаблон ---
            string mask = "+7 (###) ###-##-##";

            StringBuilder sb = new StringBuilder();
            int currNum = 0;
            foreach (char c in mask)
            {
                if (c == '#')
                {
                    sb.Append(normalizedNumber[currNum]);
                    currNum++;
                }
                else
                {
                    sb.Append(c);
                }
            }
            string result = sb.ToString();



            // --- Проверка Post-условий ---
            const string correctLengthCheck = "Корректная длина номера";
            const string correctCountryCodeCheck = "Корректный код страны";
            const string noTemplateCharsCheck = "Отсутствуют символы шаблона";


            if (result.Length != 18)
            {
                pr.AddPost(correctLengthCheck, false);
                pr.isSuccess = false;
            }
            else
            {
                pr.AddPost(correctLengthCheck, true);
            }

            if (!result.StartsWith("+7 "))
            {
                pr.AddPost(correctCountryCodeCheck, false);
                pr.isSuccess = false;
            }
            else
            {
                pr.AddPost(correctCountryCodeCheck, true);
            }

            if (result.Contains('#'))
            {
                pr.AddPost(noTemplateCharsCheck, false);
                pr.isSuccess = false;
            }
            else
            {
                pr.AddPost(noTemplateCharsCheck, true);
            }

            
            // --- Результаты ---
            if (pr.isSuccess) 
                pr.OutputText = result;

            return pr;
        }
        private string NormalizeNumber(string number)
        {
            string digitsOnly = Regex.Replace(number, @"[^\d]", string.Empty);

            //Если начинается с 8 или 7 и длина 11, убираем первый символ
            if ((digitsOnly.StartsWith("7") || digitsOnly.StartsWith("8")) && digitsOnly.Length == 11)
            {
                digitsOnly = digitsOnly.Substring(1);
            }
            return digitsOnly;
        }

        /// <summary>
        /// Превращает сырую строку в отформатированный номер СНИЛС формата XXX-XXX-XXX XX.
        /// Автоматически собирает статистику успехов и ошибок для UI.
        /// </summary>
        /// <param name="input">Входная строка от пользователя (может содержать тире, пробелы или быть пустой).</param>
        /// <returns>Объект ProcessResult, содержащий итоговый текст и списки пройденных/проваленных условий.</returns>
        public ProcessResult SnilsMask(string input)
        {
            var pr = new ProcessResult();
            string rawInput = input ?? string.Empty;

            // --- Проверка Pre-условий ---
            const string notEmpty = "Строка содержит данные";
            const string exactDigits = "Корректное количество цифр";

            if (string.IsNullOrWhiteSpace(rawInput))
            {
                pr.AddPre(notEmpty, false);
                pr.isSuccess = false;
                return pr;
            }
            pr.AddPre(notEmpty, true);

            string digitsOnly = Regex.Replace(rawInput, @"[^\d]", string.Empty);

            if (digitsOnly.Length != 11)
            {
                pr.AddPre(exactDigits, false);
                pr.isSuccess = false;
                return pr;
            }
            pr.AddPre(exactDigits, true);

            // --- Шаблон ---
            string mask = "###-###-### ##";
            StringBuilder sb = new StringBuilder();
            int currNum = 0;

            foreach (char c in mask)
            {
                if (c == '#')
                {
                    sb.Append(digitsOnly[currNum]);
                    currNum++;
                }
                else
                {
                    sb.Append(c);
                }
            }
            string result = sb.ToString();

            // --- Проверка Post-условий ---
            const string correctLen = "Корректная длина СНИЛС";
            const string noTemplates = "Отсутствуют символы шаблона";

            bool lenOk = result.Length == 14;
            pr.AddPost(correctLen, lenOk);
            if (!lenOk) pr.isSuccess = false;

            bool tempOk = !result.Contains('#');
            pr.AddPost(noTemplates, tempOk);
            if (!tempOk) pr.isSuccess = false;

            if (pr.isSuccess)
                pr.OutputText = result;

            return pr;
        }

        /// <summary>
        /// Превращает сырую строку в отформатированный номер банковской карты формата XXXX XXXX XXXX XXXX.
        /// Автоматически собирает статистику успехов и ошибок для UI.
        /// </summary>
        /// <param name="input">Входная строка от пользователя (16 цифр, может содержать лишние символы или пробелы).</param>
        /// <returns>Объект ProcessResult, содержащий итоговый текст и списки пройденных/проваленных условий.</returns>
        public ProcessResult BankCardMask(string input)
        {
            var pr = new ProcessResult();
            string rawInput = input ?? string.Empty;

            // --- Проверка Pre-условий ---
            const string notEmpty = "Строка содержит данные";
            const string cardDigits = "Корректное количество цифр";

            if (string.IsNullOrWhiteSpace(rawInput))
            {
                pr.AddPre(notEmpty, false);
                pr.isSuccess = false;
                return pr;
            }
            pr.AddPre(notEmpty, true);

            string digitsOnly = Regex.Replace(rawInput, @"[^\d]", string.Empty);

            if (digitsOnly.Length != 16)
            {
                pr.AddPre(cardDigits, false);
                pr.isSuccess = false;
                return pr;
            }
            pr.AddPre(cardDigits, true);

            // --- Шаблон ---
            string mask = "#### #### #### ####";
            StringBuilder sb = new StringBuilder();
            int currNum = 0;

            foreach (char c in mask)
            {
                if (c == '#')
                {
                    sb.Append(digitsOnly[currNum]);
                    currNum++;
                }
                else
                {
                    sb.Append(c);
                }
            }
            string result = sb.ToString();

            // --- Проверка Post-условий ---
            const string correctLen = "Длина результирующей строки";
            const string spacesCheck = "Разделители расставлены корректно";

            bool lenOk = result.Length == 19;
            pr.AddPost(correctLen, lenOk);
            if (!lenOk) pr.isSuccess = false;

            bool spacesOk = result[4] == ' ' && result[9] == ' ' && result[14] == ' ';
            pr.AddPost(spacesCheck, spacesOk);
            if (!spacesOk) pr.isSuccess = false;

            if (pr.isSuccess)
                pr.OutputText = result;

            return pr;
        }

    }
}
