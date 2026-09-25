using TextProcessor.Logic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;

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
                this.UpdateOperationVisibility();
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

        // Выбранный тип маски
        private string selectedMaskType = "Телефон (10 цифр)";
        public string SelectedMaskType
        {
            get { return this.selectedMaskType; }
            set
            {
                this.selectedMaskType = value;
                this.Notify("SelectedMaskType");
            }
        }

        // Видимость поля фильтрации
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

        // Видимость выбора маски
        private System.Windows.Visibility isMaskingOptionsVisible = System.Windows.Visibility.Collapsed;
        public System.Windows.Visibility IsMaskingOptionsVisible
        {
            get { return this.isMaskingOptionsVisible; }
            set
            {
                this.isMaskingOptionsVisible = value;
                this.Notify("IsMaskingOptionsVisible");
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

        // Методы управления видимостью

        private void UpdateOperationVisibility()
        {
            this.IsFilteringVisible = (this.currentOperationTitle == "Фильтрация строк")
                ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            this.IsMaskingOptionsVisible = (this.currentOperationTitle == "Маскирование")
                ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
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

        // Кнопка Выполнить
        public void ExecuteOperation()
        {
            try
            {
                if (this.currentOperationTitle == "Нормализация текста")
                {
                    var normalizer = new Normalization();
                    this.inputText = normalizer.Normalize(this.inputText);
                    this.PostColor = "Green";
                    this.PostText = "ВЫПОЛНЕНО";
                }
                else if (this.currentOperationTitle == "Маскирование")
                {
                    var masker = new TextMasking();
                    ProcessResult result;

                    // Выбираем метод в зависимости от того, что выбрал пользователь в интерфейсе
                    if (this.selectedMaskType == "Телефон (10 цифр)")
                    {
                        result = masker.RuNumberMask(this.inputText);
                    }
                    else if (this.selectedMaskType == "СНИЛС (11 цифр)")
                    {
                        result = masker.SnilsMask(this.inputText);
                    }
                    else if (this.selectedMaskType == "Банковская карта (16 цифр)")
                    {
                        result = masker.BankCardMask(this.inputText);
                    }
                    else
                    {
                        result = masker.RuNumberMask(this.inputText); // запасной вариант
                    }

                    this.HandleProcessResult(result);
                }
                else if (this.currentOperationTitle == "Фильтрация строк")
                {
                    var filter = new TextFiltering();
                    string keywordToUse = string.IsNullOrWhiteSpace(this.filterKeyword) ? "тест" : this.filterKeyword;
                    ProcessResult result = filter.FilterLinesByKeyword(this.inputText, keywordToUse);
                    this.HandleProcessResult(result);
                }

                this.Notify("InputText");
            }
            catch (System.ArgumentNullException ex)
            {
                this.PostColor = "Red";
                this.PostText = "ОШИБКА Pre: " + ex.Message;
            }
            catch (System.InvalidOperationException ex)
            {
                this.PostColor = "Red";
                this.PostText = "ОШИБКА Post: " + ex.Message;
            }
            catch (System.Exception ex)
            {
                this.PostColor = "Red";
                this.PostText = "КРИТИЧЕСКАЯ ОШИБКА: " + ex.Message;
            }
        }

        // Вспомогательный метод для обработки результата
        private void HandleProcessResult(ProcessResult result)
        {
            var failedPres = result.PreConditions.Where(c => !c.IsMet).ToList();

            if (failedPres.Any())
            {
                this.PreColor = "Red";
                string preErrors = string.Join("; ", failedPres.Select(c => c.Name));
                this.PreText = "ОШИБКА: " + preErrors;

                this.PostColor = "Red";
                this.PostText = "НЕ ВЫПОЛНЕНО";
                return;
            }

            this.PreColor = "Green";
            this.PreText = "ВЫПОЛНЕНО";

            if (result.isSuccess == true)
            {
                this.inputText = result.OutputText;
                this.PostColor = "Green";
                this.PostText = "ВЫПОЛНЕНО";
            }
            else
            {
                this.PostColor = "Red";
                var failedPosts = result.PostConditions.Where(c => !c.IsMet).ToList();
                string postErrors = string.Join("; ", failedPosts.Select(c => c.Name));
                this.PostText = "ОШИБКА: " + postErrors;
            }

            this.Notify("InputText");
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
            // Создаем диалог открытия файла
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите текстовый файл для загрузки";
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            openFileDialog.DefaultExt = ".txt";

            // Показываем диалог и проверяем, нажал ли пользователь "ОК"
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    // Используем бизнес-логику для чтения файла
                    TextFileIO fileIO = new TextFileIO();
                    string loadedText = fileIO.ImportFromFile(openFileDialog.FileName);

                    // Записываем текст в свойство
                    this.InputText = loadedText;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Кнопка Сохранить файл
        public void SaveFile()
        {
            // Создаем диалог сохранения файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите место для сохранения файла";
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.FileName = "result.txt"; // Имя по умолчанию

            // Показываем диалог и проверяем, нажал ли пользователь "Сохранить"
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    // Используем бизнес-логику для записи файла
                    TextFileIO fileIO = new TextFileIO();
                    fileIO.ExportToFile(saveFileDialog.FileName, this.InputText);

                    MessageBox.Show("Файл успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении файла:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}