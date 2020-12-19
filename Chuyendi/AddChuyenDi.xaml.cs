using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chuyendi
{
    /// <summary>
    /// Interaction logic for AddChuyenDi.xaml
    /// </summary>
    public partial class AddChuyenDi : Window
    {
        TTChuyendi newTTChuyendi;
        public AddChuyenDi()
        {
            newTTChuyendi = new TTChuyendi() { ImgLink = "imgs/splash.jpg", Members = new List<Thanhvien>(), Place = "Nơi đến", Name = "Tên chuyến đi", Status = TTChuyendi.STATUS[0] };


            InitializeComponent();

            statusComboBox.ItemsSource = TTChuyendi.STATUS;

            //newTTChuyendi = new TTChuyendi() { ImgLink = "imgs/splash.jpg", Members = new List<Thanhvien>(), Place = "Nơi đến", Name = "Tên chuyến đi", Status=TTChuyendi.STATUS[0]};

            // Binding 
            Binding bindingName = new Binding("Name");
            bindingName.Source = newTTChuyendi;
            bindingName.Mode = BindingMode.TwoWay;
            bindingName.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            nameTxtBox.SetBinding(TextBox.TextProperty, bindingName);

            Binding bindingPlace = new Binding("Place");
            bindingPlace.Source = newTTChuyendi;
            bindingPlace.Mode = BindingMode.TwoWay;
            bindingPlace.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            placeTxtBox.SetBinding(TextBox.TextProperty, bindingPlace);

            Binding bindingStatus = new Binding("Status");
            bindingStatus.Source = newTTChuyendi;
            bindingStatus.Mode = BindingMode.TwoWay;
            bindingStatus.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            statusComboBox.SetBinding(ComboBox.SelectedValueProperty, bindingStatus);

            Binding bindingImgLink = new Binding("ImgLink");
            bindingImgLink.Source = newTTChuyendi;
            bindingImgLink.Mode = BindingMode.TwoWay;
            bindingImgLink.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            bindingImgLink.Converter = new DirectoryConverter();
            imgLink.SetBinding(Image.TagProperty, bindingImgLink);

            imgLink.Source = new BitmapImage(new Uri(newTTChuyendi.ImgLink, UriKind.Relative));

        }
              
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        private void chooseImgBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialogCSV = new OpenFileDialog();

            openFileDialogCSV.ShowDialog();
            openFileDialogCSV.Filter = "JPEG files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialogCSV.FilterIndex = 1;
            openFileDialogCSV.RestoreDirectory = true;

            var fileName = openFileDialogCSV.FileName;
           
            if (fileName.Length > 0)
            {
                System.IO.File.Copy(fileName, AppSettings.WorkingDerectory + "imgs/cd" + AppSettings.appSettings.ImgIDCurrent + "img" + ".jpg", true);

                newTTChuyendi.ImgLink = "imgs/cd" + AppSettings.appSettings.ImgIDCurrent + "img" + ".jpg";

                imgLink.Source = new BitmapImage(new Uri(AppSettings.WorkingDerectory + newTTChuyendi.ImgLink));                
            }            
        }

        private void addTTChuyenDiBtn_Click(object sender, RoutedEventArgs e)
        {
            ChuyendiDAO.Add(newTTChuyendi);

            AppSettings.appSettings.ImgIDCurrent++;
            AppSettings.SaveSettings();
        }


        public sealed class DirectoryConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return AppSettings.WorkingDerectory + (string)value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return ((string)value).Substring(AppSettings.WorkingDerectory.Length, ((string)value).Length - AppSettings.WorkingDerectory.Length);
            }
        }
    }
}
