using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace SimpleTextEditor
{
    internal class OpenDialog : Freezable
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(OpenDialog),
            new PropertyMetadata(default(string)));

        public string Title
        {
            get => (string) GetValue(TitleProperty); 
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
            nameof(Filter),
            typeof(string),
            typeof(OpenDialog),
            new PropertyMetadata("Тестовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"));

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public static readonly DependencyProperty SelectedFileProperty = DependencyProperty.Register(
            nameof(SelectedFile),
            typeof(string),
            typeof(OpenDialog),
            new PropertyMetadata(default(string)));
        public string SelectedFile
        {
            get => (string)GetValue(SelectedFileProperty);
            set => SetValue(SelectedFileProperty, value);
        }

        public  ICommand OpenCommand { get; }

        public OpenDialog()
        {
            OpenCommand = new RelayCommand(OnOpenCommandExecuted);
        }

        private void OnOpenCommandExecuted(object parameter)
        {
            var dialog = new OpenFileDialog()
            {
                Title = Title,
                Filter = Filter,
                RestoreDirectory = true,
                InitialDirectory = Environment.CurrentDirectory,
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            SelectedFile = dialog.FileName;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new OpenDialog();
        }
    }
}
