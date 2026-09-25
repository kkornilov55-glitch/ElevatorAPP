using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Свойства для интерфейса
        private string currentOperationTitle = "Нормализация текста";
        public string CurrentOperationTitle
        {
            get
            {
                return this.currentOperationTitle;
            }
            set
            {
                this.currentOperationTitle = value;
                this.Notify("CurrentOperationTitle");
                this.ResetStatuses();
            }
        }

        private string inputText = "";
        public string InputText
        {
            get
            {
                return this.inputText;
            }
            set
            {
                this.inputText = value;
                this.Notify("InputText");
                this.CheckPreCondition(); // Проверяем Pre при каждом изменении текста
            }
        }

        // Индикаторы Pre
        private string preColor = "Red";
        public string PreColor
        {
            get { return this.preColor; }
            set
            {
                this.preColor = value;
                this.Notify("PreColor");
            }
        }

        private string preText = "НЕ ВЫПОЛНЕНО";
        public string PreText
        {
            get { return this.preText; }
            set
            {
                this.preText = value;
                this.Notify("PreText");
            }
        }

        // Индикаторы Post
        private string postColor = "Red";
        public string PostColor
        {
            get { return this.postColor; }
            set
            {
                this.postColor = value;
                this.Notify("PostColor");
            }
        }

        private string postText = "НЕ ВЫПОЛНЕНО";
        public string PostText
        {
            get { return this.postText; }
            set
            {
                this.postText = value;
                this.Notify("PostText");
            }
        }

        // Кнопки слева
        public void SelectNormalization()
        {
            this.CurrentOperationTitle = "Нормализация текста";
        }

        public void SelectMasking()
        {
            this.CurrentOperationTitle = "Маскирование";
        }

        public void SelectFiltering()
        {
            this.CurrentOperationTitle = "Фильтрация строк";
        }

        // Логика статусов
        private void ResetStatuses()
        {
            this.PreColor = "Red";
            this.PreText = "НЕ ВЫПОЛНЕНО";
            this.PostColor = "Red";
            this.PostText = "НЕ ВЫПОЛНЕНО";
            this.CheckPreCondition();
        }

        private void CheckPreCondition()
        {
            // пример заменить на бизнес-логику потом
            if (string.IsNullOrWhiteSpace(this.inputText))
            {
                this.PreColor = "Red";
                this.PreText = "НЕ ВЫПОЛНЕНО";
            }
            else
            {
                this.PreColor = "Green";
                this.PreText = "ВЫПОЛНЕНО";
            }
        }

        // Кнопка Выполнить (поставить потом сюда бизнес-логику, это щас для теста)
        public void ExecuteOperation()
        {
            // Проверка Pre-условия
            if (string.IsNullOrWhiteSpace(this.inputText))
            {
                this.PostColor = "Red";
                this.PostText = "ОШИБКА: Pre-условие не выполнено (текст пуст)";
                return;
            }

            if (this.currentOperationTitle == "Нормализация текста")
            {
                // Имитация нормализации
                this.inputText = this.inputText.Trim().ToLower();
            }
            else if (this.currentOperationTitle == "Маскирование")
            {
                // Имитация маскирования
                this.inputText = this.inputText.Replace("а", "*").Replace("о", "*").Replace("е", "*").Replace("и", "*");
            }
            else if (this.currentOperationTitle == "Фильтрация строк")
            {
                // Имитация фильтрации
                this.inputText = this.inputText.Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "").Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "");
            }

            // Сообщаем интерфейсу, что текст изменился
            this.Notify("InputText");

            // Проверка Post-условия
            this.PostColor = "Green";
            this.PostText = "ВЫПОЛНЕНО";
        }

        // Кнопка Показать контракт
        public void ShowContract()
        {
            ContractViewModel contractViewModel = new ContractViewModel(this.currentOperationTitle);
            ContractWindow contractWindow = new ContractWindow(contractViewModel);
            contractWindow.ShowDialog();
        }

        // Кнопка Загрузить файл
        public void LoadFile()
        {
            // var filePath = fileService.OpenFileDialog();
            // if (filePath != null) this.InputText = fileService.ReadFile(filePath);
            MessageBox.Show("Заглушка: Здесь откроется проводник для выбора файла", "Загрузить файл");
        }

        // Кнопка Сохранить файл
        public void SaveFile()
        {
            // var filePath = fileService.SaveFileDialog();
            // if (filePath != null) fileService.WriteFile(filePath, this.InputText);
            MessageBox.Show("Заглушка: Здесь откроется проводник для сохранения файла", "Скачать файл");
        }
    }
}