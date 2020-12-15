using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        Timer timer;
        public Splash()
        {
            InitializeComponent();

            timer = new Timer(3000);

            timer.Elapsed += ToMainWindows;

            string WorkingDerectory = System.IO.Directory.GetCurrentDirectory().Replace('\\', '/') + "/";
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();

        }

        private void ToMainWindows(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => {
                timer.Stop();

                /*if (offSplashCheckBox.IsChecked == true)
                {
                    AppConfig.appconfig.Splash = false;
                    AppConfig.Update();
                }*/

                this.Hide();
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();

                this.Close();

            }));

        }
    }
}
