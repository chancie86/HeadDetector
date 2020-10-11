using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using DesktopApp;
using FaceDetector.ViewModels;
using Newtonsoft.Json;

namespace FaceDetector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            LoadConfig();
        }

        public AppConfig Config { get; private set; }

        public void LoadConfig()
        {
            using (StreamReader r = new StreamReader("app.settings.json"))
            {
                string json = r.ReadToEnd();
                Config = JsonConvert.DeserializeObject<AppConfig>(json);
            }
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            string initialFolder = Directory.Exists(Config.InitialFolder)
                ? Config.InitialFolder
                : AssemblyDirectory;

            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(Config)
            };

            mainWindow.Show();
        }

        private static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
