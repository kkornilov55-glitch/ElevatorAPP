using System.Text;
using System.Text.RegularExpressions;
namespace Elevator.Logic
{
    public class TextMasking
    {
        public ProcessResult RuNumberMask(string number)
        {
            var pr = new ProcessResult();

            var normalizedNumber = NormalizeNumber(number?? string.Empty);


            // --- Проверка Pre-условий ---
            const string isNotEmptyCheck = "Строка содержит данные";
            const string digitCountCheck = "Корректное количество цифр";

            if (string.IsNullOrWhiteSpace(normalizedNumber))
            {
                pr.AddPre(isNotEmptyCheck, false);
                pr.isSuccess = false;
            }
            else
            {
                pr.AddPre(isNotEmptyCheck, true);
            }

            if (normalizedNumber.Length != 10)
            {
                pr.AddPre(digitCountCheck, false);
                pr.isSuccess = false;
            }
            else
            {
                pr.AddPre(digitCountCheck, true);
            }


            // --- Приводим номер к шаблонной записи --

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
            //Оставляем только цифры и убираем код страны
            return Regex.Replace(number, @"[^\d]", string.Empty).Substring(1);
        }


    }
}
