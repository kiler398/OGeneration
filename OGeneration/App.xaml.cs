using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CommonServiceLocator;
using log4net;
using log4net.Repository.Hierarchy;
using OGeneration.View;
using OGeneration.ViewModel;

namespace OGeneration
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static ILog _logger = LogManager.GetLogger(typeof(App));

 

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");

            TaskScheduler.UnobservedTaskException += (s, e) =>
                LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
 
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";
            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, new Exception("Exception in LogUnhandledException"));
            }
            finally
            {
                _logger.Error(exception, new Exception(message));
            }
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
 


            MainWindow mdi;
            try
            {
                mdi = new MainWindow(Environment.CurrentDirectory, e.Args.ToArray());

                this.MainWindow = mdi;

                SetupExceptionHandling();
            }
            catch (Exception ex)
            {
                mdi = null;
            }

            if (mdi != null)
            {
                mdi.Show();
            }

        }
    }
}
