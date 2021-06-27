using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using TimeDate_Application.Model;
using TimeDate_Application.ViewModel;

namespace TimeDate_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            this.Loaded += MainWindow_Initialized;
            this.DataContext = new TimeDate();
        }


        public void Format_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (int.TryParse(this.Date_Text.Text.Replace(".", ""), out int res))
            {
                Settings.Instance.Format = true;
                this.Date_Text.Text = DateTime.Parse(this.Date_Text.Text).ToString("dd MMMM yyyy");
            }
            else
            {
                Settings.Instance.Format = false;
                this.Date_Text.Text = DateTime.Parse(this.Date_Text.Text).ToShortDateString();
            }
        }

        public void DateColor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.ShowDialog();
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(dialog.Color.A,dialog.Color.R,dialog.Color.G,dialog.Color.B);
            Settings.Instance.Date_color = brush.Color.ToString();
            this.Date_Text.Foreground = brush;
        }

        public void TimeColor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.ShowDialog();
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
            Settings.Instance.Time_color = brush.Color.ToString();
            this.Time_Text.Foreground = brush;
        }

        public void Background_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.ShowDialog();
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
            Settings.Instance.Bg_color = brush.Color.ToString();
            this.TimeDate_DockPanel.Background = brush;
        }

        public void Autorun_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",true);
            key.SetValue("Alarm Clock", AppDomain.CurrentDomain.BaseDirectory + "TimeDate.exe");
        }

        public void DarkTheme_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(App.Current.Resources.MergedDictionaries[0].Source != new Uri("darktheme.xaml", UriKind.Relative))
            {
                Settings.Instance.Dark_theme = true;
                this.Theme.Content = "Light Theme";
                App.Current.Resources.MergedDictionaries[0].Source = new Uri("darktheme.xaml", UriKind.Relative);
            }
            else
            {
                Settings.Instance.Dark_theme = false;
                this.Theme.Content = "Dark Theme";
                App.Current.Resources.MergedDictionaries[0].Source = new Uri("lighttheme.xaml", UriKind.Relative);
            }
        }

        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            string settings_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Alarm Clock";
            if (!Directory.Exists(settings_path))
            {
                Directory.CreateDirectory(settings_path);
            }
            else if (File.Exists(settings_path + "\\settings.xml"))
            {
                using (var reader = new StreamReader((settings_path + "\\settings.xml")))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    Settings.Instance = (serializer.Deserialize(reader) as Settings);
                }
                Settings.Instance.Load(this, e);
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Instance.Save();
        }
    }
}
