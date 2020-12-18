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
        public static string color_notchoose = "#ff23aa50";

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
                listChuyendi = ChuyendiDAO.GetAll().Where(ele => ele.Status > 3).ToList();
            }
            else
            if (FilterNotFinished)
            {
                listChuyendi = ChuyendiDAO.GetAll().Where(ele => ele.Status <= 3).ToList();
            }
            else
            {
                listChuyendi = ChuyendiDAO.GetAll();
            }

            //search
            // do something


            chuyendiListView.ItemsSource = listChuyendi;

        }

        private void filterFinishedBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var bc = new BrushConverter();

            // Change color when click
            if (filterFinishedBtn.Background.ToString().ToLower().Equals(color_choose))
            {
                filterFinishedBtn.Background = (Brush)bc.ConvertFrom(color_notchoose);
                FilterFinished = false;
                
            }
            else
            {
                filterFinishedBtn.Background = (Brush)bc.ConvertFrom(color_choose);
                filterNotFinishedBtn.Background = (Brush)bc.ConvertFrom(color_notchoose);
                FilterNotFinished = false;
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
                FilterNotFinished = false;
            }
            else
            {
                filterNotFinishedBtn.Background = (Brush)bc.ConvertFrom(color_choose);
                filterFinishedBtn.Background = (Brush)bc.ConvertFrom(color_notchoose);
                FilterFinished = false;
                FilterNotFinished = true;
            }
            UpdateListSource();

            Debug.WriteLine("" + FilterFinished + FilterNotFinished);
        }

    }
    public class StatusChuyenDiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return TTChuyendi.STATUS[Int32.Parse(value.ToString())];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            for (int i = 0; i < 5; i++)
            {
                if (value.ToString().Equals(TTChuyendi.STATUS[i])) return i;
            }

            return 0;
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
}
