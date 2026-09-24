using System.ComponentModel;

namespace UI.ViewModels
{
    public class ContractViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string title = "";
        public string Title
        {
            get { return this.title; }
            set { this.title = value; this.Notify("Title"); }
        }

        private string preCondition = "";
        public string PreCondition
        {
            get { return this.preCondition; }
            set { this.preCondition = value; this.Notify("PreCondition"); }
        }

        private string postCondition = "";
        public string PostCondition
        {
            get { return this.postCondition; }
            set { this.postCondition = value; this.Notify("PostCondition"); }
        }

        private string effects = "";
        public string Effects
        {
            get { return this.effects; }
            set { this.effects = value; this.Notify("Effects"); }
        }

        private string examples = "";
        public string Examples
        {
            get { return this.examples; }
            set { this.examples = value; this.Notify("Examples"); }
        }

        public ContractViewModel(string operationTitle)
        {
            this.BuildContract(operationTitle);
        }

        private void BuildContract(string operationTitle)
        {
            this.Title = "Контракт: " + operationTitle;

            if (operationTitle == "Нормализация текста")
            {
                this.PreCondition = "1. Входная строка не является null\n" + 
                                    "2. Тип переданного значения — строка (string)";
                this.PostCondition = "1. Возвращаемое значение не равно null\n" + 
                                     "2. Возвращаемая строка полностью приведена к нижнему регистру\n" +
                                     "3. Возвращаемая строка не содержит двойных пробелов подряд\n" +
                                     "4. Возвращаемая строка не содержит знаков препинания и спецсимволов\n" +
                                     "5. Если на вход была подана пустая строка, на выходе также будет пустая строка";
                this.Effects = "Исходная строка не изменяется. Возвращается новая строка. Если входная строка null — выбрасывается ArgumentException.";
                this.Examples = "1. \"  ПРИВЕТ  Мир  \" -> \"привет мир\" (Pre OK, Post OK)\n" +
                                "2. \"Привет, мир!\" -> \"привет мир\" (Pre OK, Post OK)\n" +
                                "3. \"\" -> \"\" (Pre OK, Post OK — пустая остаётся пустой)\n" +
                                "4. null -> ArgumentException (Pre FAIL)\n" +
                                "5. \"АБВГД!!!\" -> \"абвгд\" (Pre OK, Post OK)\n" +
                                "6. \"Тест... 123\" -> \"тест 123\" (Pre OK, Post OK)";
            }
            else if (operationTitle == "Маскирование")
            {
                this.PreCondition = "Входная строка не является null.";
                this.PostCondition = "Все гласные буквы русского алфавита (а, о, е, и, у, ы, э, я, ю, А, О, Е, И, У, , Э, Я, Ю) в результирующей строке заменены на символ '*'.";
                this.Effects = "Исходная строка не изменяется. Возвращается новая строка. Если входная строка null — выбрасывается ArgumentException.";
                this.Examples = "1. \"привет\" -> \"пр*в*т\" (Pre OK, Post OK)\n" +
                                "2. \"\" -> \"\" (Pre OK, Post OK)\n" +
                                "3. null -> ArgumentException (Pre FAIL)\n" +
                                "4. \"АЕИОУ\" -> \"*****\" (Pre OK, Post OK)\n" +
                                "5. \"нет гласных\" -> \"нет гласных\" (Pre OK, Post OK)";
            }
            else if (operationTitle == "Фильтрация строк")
            {
                this.PreCondition = "Входная строка не является null.";
                this.PostCondition = "Результирующая строка не содержит цифровых символов (0-9). Все остальные символы сохранены.";
                this.Effects = "Исходная строка не изменяется. Возвращается новая строка. Если входная строка null — выбрасывается ArgumentException.";
                this.Examples = "1. \"Тест 123 пример\" -> \"Тест  пример\" (Pre OK, Post OK)\n" +
                                "2. \"нет цифр\" -> \"нет цифр\" (Pre OK, Post OK)\n" +
                                "3. null -> ArgumentException (Pre FAIL)\n" +
                                "4. \"12345\" -> \"\" (Pre OK, Post OK)\n" +
                                "5. \"\" -> \"\" (Pre OK, Post OK)";
            }
            else
            {
                this.PreCondition = "Неизвестная операция.";
                this.PostCondition = "Неизвестная операция.";
                this.Effects = "Нет данных.";
                this.Examples = "Нет данных.";
            }
        }
    }
}