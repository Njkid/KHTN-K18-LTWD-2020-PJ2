using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
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
            this.DataContext = this;
            InitializeComponent();
            IDTTChuyendi = IDChuyendi;
            TTChuyendi thisCD = ChuyendiDAO.GetAll()[IDChuyendi];
            listViewMembers.ItemsSource = ChuyendiDAO.GetAll()[IDChuyendi].Members;

            // Binding name TTChuyendi
            Binding bindingNameCD = new Binding("Name");
            bindingNameCD.Source = ChuyendiDAO.GetAll()[IDChuyendi];
            bindingNameCD.Mode = BindingMode.TwoWay;
            bindingNameCD.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            nameCDTextBlock.SetBinding(TextBlock.TextProperty, bindingNameCD);

            // Binding place TTChuyendi
            Binding bindingPlaceCD = new Binding("Place");
            bindingPlaceCD.Source = ChuyendiDAO.GetAll()[IDChuyendi];
            bindingPlaceCD.Mode = BindingMode.TwoWay;
            bindingPlaceCD.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            placeTextBlock.SetBinding(TextBlock.TextProperty, bindingPlaceCD);

            imageImg.Source = new BitmapImage(new Uri(AppSettings.WorkingDerectory + thisCD.ImgLink));


            // Binding total TTChuyendi
            Binding bindingTotalCD = new Binding("Total");
            bindingTotalCD.Source = ChuyendiDAO.GetAll()[IDChuyendi];
            bindingTotalCD.Mode = BindingMode.TwoWay;
            bindingTotalCD.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            totalTextBlock.SetBinding(TextBlock.TextProperty, bindingTotalCD);

            // Binding avg TTChuyendi
            Binding bindingAvgCD = new Binding("Avg");
            bindingAvgCD.Source = ChuyendiDAO.GetAll()[IDChuyendi];
            bindingAvgCD.Mode = BindingMode.TwoWay;
            bindingAvgCD.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            avgTextBlock.SetBinding(TextBlock.TextProperty, bindingAvgCD);

            //Binding Combobox
            statusChangeComboBox.ItemsSource = TTChuyendi.STATUS;

            Binding bindingStatus = new Binding("Status");
            bindingStatus.Source = ChuyendiDAO.GetAll()[IDChuyendi];
            bindingStatus.Mode = BindingMode.TwoWay;
            bindingStatus.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            statusChangeComboBox.SetBinding(ComboBox.SelectedValueProperty, bindingStatus);



            Binding bindingMem = new Binding("Members");              
            bindingMem.Source = ChuyendiDAO.GetAll()[IDChuyendi];            
            bindingMem.Mode = BindingMode.TwoWay;
            bindingMem.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            chartController.SetBinding(PieSeries.ItemsSourceProperty, bindingMem);
           
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void UpdateList()
        {
            listViewMembers.ItemsSource = ChuyendiDAO.GetAll()[IDTTChuyendi].Members;
            listViewMembers.Items.Refresh();

            // Binding avg TTChuyendi
            Binding bindingAvgCD = new Binding("Avg");
            bindingAvgCD.Source = ChuyendiDAO.GetAll()[IDTTChuyendi];
            bindingAvgCD.Mode = BindingMode.TwoWay;
            bindingAvgCD.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            avgTextBlock.SetBinding(TextBlock.TextProperty, bindingAvgCD);

            // Binding total TTChuyendi
            Binding bindingTotalCD = new Binding("Total");
            bindingTotalCD.Source = ChuyendiDAO.GetAll()[IDTTChuyendi];
            bindingTotalCD.Mode = BindingMode.TwoWay;
            bindingTotalCD.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            totalTextBlock.SetBinding(TextBlock.TextProperty, bindingTotalCD);

            Binding bindingMem = new Binding("Members");
            bindingMem.Source = ChuyendiDAO.GetAll()[IDTTChuyendi];
            bindingMem.Mode = BindingMode.TwoWay;
            bindingMem.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            chartController.SetBinding(PieSeries.ItemsSourceProperty, bindingMem);
            chartController.Refresh();
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
            ChuyendiDAO.GetAll()[IDTTChuyendi].Members.Insert(0, new Thanhvien() { Name = "Nhập tên", Bills = new List<Bill>(), Debt = 0, Paid = 0 });
            ChuyendiDAO.GetAll()[IDTTChuyendi].Update();
            UpdateList();
        }

        ~TTChuyendiManager()
        {
            ChuyendiDAO.SaveData(ChuyendiDAO.GetAll());
        }

        private void addBillButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            ChuyendiDAO.GetAll()[IDTTChuyendi].Members.Where(mem => mem.Name.Equals(btn.Tag)).ToList()[0].Bills.Insert(0, new Bill());
            ChuyendiDAO.GetAll()[IDTTChuyendi].Update();
            UpdateList();
        }

        private void costTxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ChuyendiDAO.GetAll()[IDTTChuyendi].Update();
            UpdateList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundTTChuyendi.ImageSource = new BitmapImage(new Uri(AppSettings.WorkingDerectory + ChuyendiDAO.GetAll()[IDTTChuyendi].ImgLink));
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

    // tham khảo https://stackoverflow.com/questions/47993115/chartingtoolkitchart-how-to-show-value-of-each-pie
    public class PieDataPoint : System.Windows.Controls.DataVisualization.Charting.PieDataPoint
    {
        public static readonly DependencyProperty TextedGeometryProperty =
            DependencyProperty.Register("TextedGeometry", typeof(Geometry), typeof(PieDataPoint));

        public Geometry TextedGeometry
        {
            get { return (Geometry)GetValue(TextedGeometryProperty); }
            set { SetValue(TextedGeometryProperty, value); }
        }

        static PieDataPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PieDataPoint),
                new FrameworkPropertyMetadata(typeof(PieDataPoint)));
        }

        public PieDataPoint()
        {
            DependencyPropertyDescriptor dependencyPropertyDescriptor
                = DependencyPropertyDescriptor.FromProperty(GeometryProperty, GetType());

            dependencyPropertyDescriptor.AddValueChanged(this, OnGeometryValueChanged);
        }

        private double LabelFontSize
        {
            get
            {
                FrameworkElement parentFrameworkElement = Parent as FrameworkElement;
                return Math.Max(8, Math.Min(parentFrameworkElement.ActualWidth,
                    parentFrameworkElement.ActualHeight) / 30);
            }
        }

        private void OnGeometryValueChanged(object sender, EventArgs arg)
        {
            Point point;
            FormattedText formattedText;

            CombinedGeometry combinedGeometry = new CombinedGeometry();
            combinedGeometry.GeometryCombineMode = GeometryCombineMode.Exclude;

            formattedText = new FormattedText(FormattedRatio,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                LabelFontSize,
                Brushes.White);

            if (ActualRatio == 1)
            {
                EllipseGeometry ellipseGeometry = Geometry as EllipseGeometry;

                point = new Point(ellipseGeometry.Center.X - formattedText.Width / 2,
                    ellipseGeometry.Center.Y - formattedText.Height / 2);
            }
            else if (ActualRatio == 0)
            {
                TextedGeometry = null;
                return;
            }
            else
            {
                Point tangent;
                Point half;
                Point origin;

                PathGeometry pathGeometry = Geometry as PathGeometry;
                pathGeometry.GetPointAtFractionLength(.5, out half, out tangent);
                pathGeometry.GetPointAtFractionLength(0, out origin, out tangent);

                point = new Point(origin.X + ((half.X - origin.X) / 2) - formattedText.Width / 2,
                    origin.Y + ((half.Y - origin.Y) / 2) - formattedText.Height / 2);

            }

            combinedGeometry.Geometry1 = Geometry;
            combinedGeometry.Geometry2 = formattedText.BuildGeometry(point);

            TextedGeometry = combinedGeometry;
        }
    }
    

    public class Test {
        public string Name { get; set; }
        public int Paid { get; set; }
    }
}
