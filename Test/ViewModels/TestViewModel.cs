using System.Text.Json.Serialization;
using Shared.Attributes;
using Shared.Commands;
using Shared.ViewModels;
using System.IO;
using Microsoft.Win32;
using System.Text.Json;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;


using Shared.Commands;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace Test.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        public string TestDataPath { get; set; }
        public RelayCommand GetImageButtonCommand { get; set; }

        private BitmapImage _selectedImage;
        public BitmapImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }
        private void ExecuteClickMe(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"; 
            bool? result = dialog.ShowDialog(); 

            if (result == true)
            {
                SelectedImage = new BitmapImage(new Uri(dialog.FileName));
            }
        }
        private bool CanExecuteClickMe(object obj)
        {
            return true;
        }
        public TestViewModel()
        {
            GetImageButtonCommand = new RelayCommand(ExecuteClickMe, CanExecuteClickMe);
        }
    }
}
