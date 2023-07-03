using System;
using System.IO;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;

namespace SimpleTextEditor
{
    internal class MainWindowViewModel : ViewModel
    {
        private string _Text;

        public string Text
        {
            get => _Text;
            set => Set(ref _Text, value);
        }

        private string _FileName;

        public string FileName
        {
            get => _FileName;
            set
            {
                if (Set(ref _FileName, value)) // Если свойство с именем файла успешно обновилось, то вызываем метод чтения файла.
                {
                    ReadFileAsync(value);
                }
            }
        }

        public ICommand CreateCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand QuitCommand { get; } = new RelayCommand(p => Application.Current.Shutdown());

        public MainWindowViewModel()
        {
            CreateCommand = new RelayCommand(OnCreateCommandExecuted);
            SaveCommand = new RelayCommand(OnSaveCommandExecutedAsync, OnSaveCommandCanExecuted);
        }

        private async void OnSaveCommandExecutedAsync(object filePath)
        {
            var file_name = filePath as string;
            if (file_name is null)
            {
                var dialog = new SaveFileDialog()
                {
                    Title = "Сохранение файла...",
                    Filter = "Тестовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                    InitialDirectory = Environment.CurrentDirectory,
                    RestoreDirectory = true,
                };
                if (dialog.ShowDialog() != true)
                {
                    return;
                }
                file_name = dialog.FileName;
            }

            using (var writer = new StreamWriter(new FileStream(file_name, FileMode.Create, FileAccess.Write)))
            {
                await writer.WriteAsync(_Text).ConfigureAwait(true);
            }
            FileName = file_name;
        }

        private bool OnSaveCommandCanExecuted(object filePath)
        {
            return !string.IsNullOrEmpty(_Text);
        }

        private void OnCreateCommandExecuted(object parameter)
        {
            Text = string.Empty;
            FileName = null;
        }

        private async void ReadFileAsync(string filePath)
        {
            Text = string.Empty;

            if (!File.Exists(filePath))
            {
                return;
            }

            using (var reader = File.OpenText(filePath))
            {
                Text = await reader.ReadToEndAsync().ConfigureAwait(true); // Результат операции необходимо поместить в тот же контекст синхронизации из которого метод вызывался.
            }
        }
    }
}
