using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for TTChuyendiManager.xaml
    /// </summary>
    public partial class TTChuyendiManager : Window
    {
        public int IDTTChuyendi { get; set; }
        public TTChuyendiManager(int IDChuyendi)
        {
            InitializeComponent();
            IDTTChuyendi = IDChuyendi;
            listViewMembers.ItemsSource = ChuyendiDAO.GetAll()[IDChuyendi].Members;

            //Binding Combobox
            statusChangeComboBox.ItemsSource = TTChuyendi.STATUS;

            Binding bindingStatus = new Binding("Status");
            bindingStatus.Source = ChuyendiDAO.GetAll()[IDChuyendi];
            bindingStatus.Mode = BindingMode.TwoWay;
            bindingStatus.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            statusChangeComboBox.SetBinding(ComboBox.SelectedValueProperty, bindingStatus);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void UpdateList()
        {
            listViewMembers.ItemsSource = ChuyendiDAO.GetAll()[IDTTChuyendi].Members;
            listViewMembers.Items.Refresh();
        }

        // preference https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void costTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        public class DoubleConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if ((double)value != 0)
                {
                    return value.ToString();
                }
                return "";
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                string temp = (string)value;
                if (temp.Length > 0)
                {

                    double result = 0;
                    try
                    {
                        result = Double.Parse(temp);
                    }
                    catch
                    {
                        result = 0;
                    }
                    return result;
                }

                return 0;
            }
        }

        private void costTxtBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void addMemberBtn_Click(object sender, RoutedEventArgs e)
        {
            ChuyendiDAO.GetAll()[IDTTChuyendi].Members.Add(new Thanhvien() {Name = "Nhập tên", Bills= new List<Bill>(), Debt=0, Paid=0});
            ChuyendiDAO.GetAll()[IDTTChuyendi].Update();
            UpdateList();
        }

        ~TTChuyendiManager()
        {
            ChuyendiDAO.SaveData(ChuyendiDAO.GetAll());
        }
    }

    public sealed class ColorDebtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var bc = new BrushConverter();
            if ((double)value < 0.0001f)
            {
                return (Brush)bc.ConvertFrom("#236450");
            }
            return (Brush)bc.ConvertFrom("#ffff0000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

}
