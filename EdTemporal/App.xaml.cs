using EdTemporal.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EdTemporal {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public ServiceProvider ServiceProvider;
        
        private void Application_Startup(object sender, StartupEventArgs e) {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // -----

            Data.CommodityHandler dataHandler = new Data.CommodityHandler();
            dataHandler.UpdateAllCommodities();
        }


        private void ConfigureServices(IServiceCollection services) {
            // 
        }
    }
}
