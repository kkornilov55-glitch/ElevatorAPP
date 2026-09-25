namespace Elevator.Logic
{
    /// <summary>
    /// Класс для фильтрации текста по ключевому слову.
    /// </summary>
    public class TextFiltering
    {
        /// <summary>
        /// Фильтрует многострочный текст, оставляя только те строки, в которых встречается заданное ключевое слово.
        /// </summary>
        /// <param name="inputText">Исходный текст (может состоять из нескольких строк).</param>
        /// <param name="keyword">Слово для поиска.</param>
        /// <returns>Объект ProcessResult, содержащий отфильтрованный текст и статусы условий.</returns>
        public ProcessResult FilterLinesByKeyword(string inputText, string keyword)
        {
            var pr = new ProcessResult();
            string rawInput = inputText ?? string.Empty;
            string rawKeyword = keyword ?? string.Empty;

            // --- Проверка Pre-условий ---
            const string textNotEmpty = "Исходный текст не пуст";
            const string keywordNotEmpty = "Ключевое слово задано";

            if (string.IsNullOrWhiteSpace(rawInput))
            {
                pr.AddPre(textNotEmpty, false);
                pr.isSuccess = false;
            }
            else
            {
                pr.AddPre(textNotEmpty, true);
            }

            if (string.IsNullOrWhiteSpace(rawKeyword))
            {
                pr.AddPre(keywordNotEmpty, false);
                pr.isSuccess = false;
            }
            else
            {
                pr.AddPre(keywordNotEmpty, true);
            }

            //Если pre-условия провалены, выходим сразу
            if (!pr.isSuccess) return pr;

            // --- Логика фильтрации ---
            var lines = rawInput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Оставляем только те строки, где есть совпадение (без учета регистра)
            var filteredLines = lines
                .Where(line => line.IndexOf(rawKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            string result = string.Join(Environment.NewLine, filteredLines);

            // --- Проверка Post-условий ---
            const string foundMatches = "Найдены совпадения в тексте";
            const string noEmptyResult = "Результирующий текст не пуст";

            bool hasMatches = filteredLines.Count > 0;
            pr.AddPost(foundMatches, hasMatches);
            if (!hasMatches) pr.isSuccess = false;

            bool isResultNotEmpty = !string.IsNullOrWhiteSpace(result);
            pr.AddPost(noEmptyResult, isResultNotEmpty);
            if (!isResultNotEmpty) pr.isSuccess = false;

            // --- Результаты ---
            if (pr.isSuccess)
                pr.OutputText = result;

            return pr;
        }
    }
}
