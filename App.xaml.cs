using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config",ConfigFileExtension = "config", Watch = true)]
namespace WenElevating.FileShared
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly new App Current = (App)Application.Current;

        public IServiceProvider? ServiceProvider { get; private set; }

        public App()
        {
            InitializeComponent();
            InitializeService();
        }

        private void InitializeService()
        {
            IServiceCollection descriptors = new ServiceCollection();
            ServiceProvider = descriptors.BuildServiceProvider();
        }
    }

}
