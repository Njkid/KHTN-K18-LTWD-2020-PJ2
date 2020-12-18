using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Xml.Serialization;

namespace Chuyendi
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        Timer timer;
        public bool EndTimer { get; set; }
        public Splash()
        {
            AppSettings.LoadSettings();

            InitializeComponent();

            // Binding to CheckBox
            Binding binding = new Binding("TurnOffSplash");
            binding.Source = AppSettings.appSettings;
            notShowAgain.SetBinding(CheckBox.IsCheckedProperty, binding);

            EndTimer = false;

            //Check case show Splash or not
            if (AppSettings.appSettings.TurnOffSplash == false)
            {
                timer = new Timer(3000);
            }
            else
            {
                timer = new Timer(10);
            }            

            timer.Elapsed += ToMainWindows;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void ToMainWindows(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => {
                timer.Stop();

                if (notShowAgain.IsChecked == true)
                {
                    AppSettings.appSettings.TurnOffSplash = true;                    
                }

                AppSettings.SaveSettings();

                if (!(EndTimer))
                {
                    this.Hide();
                    EndTimer = true;
                    Debug.WriteLine(EndTimer);
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.ShowDialog();                  

                    this.Close();
                } else
                {
                    // Do nothing
                }

            }));

        }
    }

    public class AppSettings
    {
        public static AppSettings appSettings;
        public static string WorkingDerectory = System.IO.Directory.GetCurrentDirectory().Replace('\\', '/') + "/";

        public bool TurnOffSplash { get; set; }

        public static void LoadSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            string SettingsLink = WorkingDerectory + "appsettings.txt";

            try
            {
                using (Stream reader = new FileStream(SettingsLink, FileMode.Open))
                {                     
                   appSettings = (AppSettings)serializer.Deserialize(reader);                                      
                }
            }
            catch (IOException ioex)
            {
                appSettings = new AppSettings() { TurnOffSplash = false };
            }

            

        }

        public static void SaveSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            string SettingsLink = WorkingDerectory + "appsettings.txt";

            TextWriter writer = new StreamWriter(SettingsLink);
            serializer.Serialize(writer, appSettings);
            writer.Close();
        }

        ~AppSettings()
        {
            SaveSettings();
        }
    }
}
