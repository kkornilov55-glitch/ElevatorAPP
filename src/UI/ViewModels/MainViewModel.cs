using TextProcessor.Logic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace UI.ViewModels
{
    public class ConditionUiModel
    {
        public string Name { get; set; }
        public bool IsMet { get; set; }
    }

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
                this.UpdateFilterVisibility();
                this.ResetStatuses();
            }
        }

        // Свойства для управления видимостью поля фильтрации
        private System.Windows.Visibility isFilteringVisible = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility IsFilteringVisible
        {
            get { return this.isFilteringVisible; }
            set
            {
                this.isFilteringVisible = value;
                this.Notify("IsFilteringVisible");
            }
        }

        private void UpdateFilterVisibility()
        {
            if (this.currentOperationTitle == "Фильтрация строк")
            {
                this.IsFilteringVisible = System.Windows.Visibility.Visible;
            }
            else
            {
                this.IsFilteringVisible = System.Windows.Visibility.Collapsed;
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

        // Ключевое слово для фильтрации
        private string filterKeyword = "";
        public string FilterKeyword
        {
            get { return this.filterKeyword; }
            set
            {
                this.filterKeyword = value;
                this.Notify("FilterKeyword");
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
            if (string.IsNullOrWhiteSpace(this.inputText))
            {
                this.PreColor = "Red";
                this.PreText = "НЕ ВЫПОЛНЕНО (пусто)";
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
            try
            {
                if (this.currentOperationTitle == "Нормализация текста")
                {
                    // Метод возвращает строку и кидает исключения при ошибке
                    var normalizer = new Normalization();
                    this.inputText = normalizer.Normalize(this.inputText);

                    this.PostColor = "Green";
                    this.PostText = "ВЫПОЛНЕНО";
                }
                else if (this.currentOperationTitle == "Маскирование")
                {
                    // Метод возвращает ProcessResult
                    var masker = new TextMasking();
                    ProcessResult result = masker.RuNumberMask(this.inputText);
                    HandleProcessResult(result);
                }
                else if (this.currentOperationTitle == "Фильтрация строк")
                {
                    // Метод возвращает ProcessResult
                    var filter = new TextFiltering();
                    // Передаем ключевое слово
                    string keywordToUse = string.IsNullOrWhiteSpace(this.filterKeyword) ? "тест" : this.filterKeyword;

                    ProcessResult result = filter.FilterLinesByKeyword(this.inputText, keywordToUse);
                    HandleProcessResult(result);
                }

                // Уведомляем UI об изменении текста
                this.Notify("InputText");
            }
            catch (ArgumentNullException ex)
            {
                // Нарушено Pre-условие
                this.PostColor = "Red";
                this.PostText = "ОШИБКА Pre: " + ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                // Нарушено Post-условие
                this.PostColor = "Red";
                this.PostText = "ОШИБКА Post: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Любая другая непредвиденная ошибка
                this.PostColor = "Red";
                this.PostText = "КРИТИЧЕСКАЯ ОШИБКА: " + ex.Message;
            }
        }

        // Вспомогательный метод для обработки результата
        private void HandleProcessResult(ProcessResult result)
        {
            if (result.isSuccess == true)
            {
                // Если всё успешно, обновляем текст и ставим зелёный статус
                this.inputText = result.OutputText;
                this.PostColor = "Green";
                this.PostText = "ВЫПОЛНЕНО";
            }
            else
            {
                // Если есть ошибки, собираем их названия в одну строку
                this.PostColor = "Red";

                string errorMessages = "";

                // Проверяем проваленные Pre-условия
                foreach (var condition in result.PreConditions)
                {
                    if (condition.IsMet == false)
                    {
                        // Если строка уже не пустая, добавляем разделитель
                        if (errorMessages != "")
                        {
                            errorMessages = errorMessages + "; ";
                        }
                        errorMessages = errorMessages + condition.Name;
                    }
                }

                // Проверяем проваленные Post-условия
                foreach (var condition in result.PostConditions)
                {
                    if (condition.IsMet == false)
                    {
                        if (errorMessages != "")
                        {
                            errorMessages = errorMessages + "; ";
                        }
                        errorMessages = errorMessages + condition.Name;
                    }
                }

                // Выводим итоговое сообщение об ошибке
                this.PostText = "ОШИБКА: " + errorMessages;
            }
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