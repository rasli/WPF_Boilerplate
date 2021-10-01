using UI.View.Login;
using UI.View.Main;
using UI.VM.Main;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow main = new MainWindow();
            (main.DataContext as MainVM).MainView = new LoginView();
            main.Show();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //UnhandledException Handle

        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //UnhandledException Handle
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Update theme
        }
    }
}
