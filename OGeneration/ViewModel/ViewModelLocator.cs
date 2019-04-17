/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:CNF.WPFDemo"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using OGeneration.View;
using WPF.Common.ViewModels;
using WPF.Common.WindsorAdapter;

namespace OGeneration.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {

        public static WindsorContainer container;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            container = new WindsorContainer();

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            container.Register(
                Component.For<IDialogProvider>() //接口
                    .ImplementedBy<MainWindow>() //实现类
            );

            container.Register(
                Component.For<MainWindowViewModel>()
            );

 
        }

        public MainWindow MainWindow
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IDialogProvider>() as MainWindow;
            }
        }

        public MainWindowViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainWindowViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}