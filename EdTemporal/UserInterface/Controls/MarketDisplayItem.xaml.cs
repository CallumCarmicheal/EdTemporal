using EdTemporal.UserInterface.ViewModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EdTemporal.UserInterface.Controls {
    /// <summary>
    /// Interaction logic for MarketDisplayItem.xaml
    /// </summary>
    public partial class MarketDisplayItem : UserControl {
        public ControlMarketDisplayItem VM { get => (ControlMarketDisplayItem)DataContext; set => DataContext = value; }

        public MarketDisplayItem() {
            InitializeComponent();
            VM = new ControlMarketDisplayItem();

            // -----

           
        }

        public MarketDisplayItem setDisplaySettings(string CommodityName, string Value, string Percentage) {
            this.VM.CommodityName = CommodityName;
            this.VM.Value = Value;
            this.VM.PercentageChange = Percentage;
            return this;
        }
    }
}
