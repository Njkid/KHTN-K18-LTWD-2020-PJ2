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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chuyendi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<TTChuyendi> listChuyendi;
        public static string color_choose = "#ffffffff";
        public static string color_notchoose = "#9923aa50";

        public bool FilterFinished;
        public bool FilterNotFinished;
        public MainWindow()
        {
            InitializeComponent();

            ChuyendiDAO.ConnectToVirtualDataBase();

            FilterFinished = false;
            FilterNotFinished = false;

            var bc = new BrushConverter();
            filterFinishedBtn.Background = (Brush)bc.ConvertFrom(color_notchoose);
            filterNotFinishedBtn.Background = (Brush)bc.ConvertFrom(color_notchoose);

            listChuyendi = ChuyendiDAO.GetAll();

            chuyendiListView.ItemsSource = listChuyendi;            
        }

        public void UpdateListSource()
        {
            // filter
            if (FilterFinished)
            {
                listChuyendi = ChuyendiDAO.GetAll().Where(ele => ele.Status.Equals(TTChuyendi.STATUS[4])).ToList();
            }
            else
            if (FilterNotFinished)
            {
                listChuyendi = ChuyendiDAO.GetAll().Where(ele => !ele.Status.Equals(TTChuyendi.STATUS[4])).ToList();
            }
            else
            {
                listChuyendi = ChuyendiDAO.GetAll();
            }

            //search
            if (searchTextBox.Text.Length > 0)
            {
                listChuyendi = listChuyendi.Where(ele => 
                ele.Name.ToLower().Contains(searchTextBox.Text.ToLower()) ||
                ele.Place.ToLower().Contains(searchTextBox.Text.ToLower()) ||
                ele.Members.Any(mem => mem.Name.ToLower().Contains(searchTextBox.Text.ToLower()))
                ).ToList();

                
            }


            chuyendiListView.ItemsSource = listChuyendi;
            chuyendiListView.Items.Refresh();

        }

        private void filterFinishedBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            // Change color when click
            if (filterFinishedBtn.Background.ToString().ToLower().Equals(color_choose))
            {
                filterFinishedBtn.Background = (Brush)bc.ConvertFrom(color_notchoose);
                fnTxt.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                FilterFinished = false;
                
            }
            else
            {
                filterFinishedBtn.Background = (Brush)bc.ConvertFrom(color_choose);
                filterNotFinishedBtn.Background = (Brush)bc.ConvertFrom(color_notchoose);
                FilterNotFinished = false;
                nfnTxt.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                fnTxt.Foreground = (Brush)bc.ConvertFrom("#000000");
                FilterFinished = true;
            }

            UpdateListSource();

            Debug.WriteLine("" + FilterFinished + FilterNotFinished);
        }

        private void filterNotFinishedBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();
            // Change color when click
            if (filterNotFinishedBtn.Background.ToString().ToLower().Equals(color_choose))
            {
                filterNotFinishedBtn.Background = (Brush)bc.ConvertFrom(color_notchoose);
                nfnTxt.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                FilterNotFinished = false;
            }
            else
            {
                filterNotFinishedBtn.Background = (Brush)bc.ConvertFrom(color_choose);
                filterFinishedBtn.Background = (Brush)bc.ConvertFrom(color_notchoose);
                FilterFinished = false;
                FilterNotFinished = true;
                fnTxt.Foreground = (Brush)bc.ConvertFrom("#ffffff");
                nfnTxt.Foreground = (Brush)bc.ConvertFrom("#000000");
            }
            UpdateListSource();

            Debug.WriteLine("" + FilterFinished + FilterNotFinished);
        }

        private void AddChuyenDiBtn_Click(object sender, RoutedEventArgs e)
        {
            AddChuyenDi addChuyenDiWindow = new AddChuyenDi();
            addChuyenDiWindow.ShowDialog();

            UpdateListSource();
        }

        private void searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateListSource();
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateListSource();
        }
    }

    public class FilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                {
                    return MainWindow.color_choose;
                }
                else
                {
                    return MainWindow.color_notchoose;
                }
            }            
            return MainWindow.color_notchoose;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString().ToLower().Equals(MainWindow.color_choose)) return true;            
            return false;
        }
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
