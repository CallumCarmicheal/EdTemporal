using EdTemporal.UserInterface.Controls;
using EdTemporal.UserInterface.Utilities;
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
using System.Windows.Shapes;

namespace EdTemporal.UserInterface.Windows {
    /// <summary>
    /// Interaction logic for TestScroller.xaml
    /// </summary>
    public partial class TestScroller {
        public WindowTestScrollerViewModel VM { get => (WindowTestScrollerViewModel)DataContext; set => DataContext = value; }

        private Ticker<MarketDisplayItem> ticker;
        public Data.CommodityHandler dataHandler;
        public Data.InaraPriceScraper inaraPriceScraper;

        public TestScroller() {
            InitializeComponent();
            VM = new WindowTestScrollerViewModel();

            ticker = new Ticker<MarketDisplayItem>(LayoutRoot);
            ticker.Speed = new TimeSpan(0, 0, 15);
            ticker.SeperatorSize = 40;
            ticker.ItemDisplayed += ticker_ItemDisplayed;
            ticker.Start();

            Data.CommodityHandler dataHandler = new Data.CommodityHandler();
            dataHandler.UpdateAllCommodities();

            inaraPriceScraper = new Data.InaraPriceScraper();
            inaraPriceScraper.LoadPrices();

            return;

            ticker.Items.Enqueue(new MarketDisplayItem { }.setDisplaySettings("Low Temperture Diamonds", "1,054,240 cr", "6.2% ▲"));
            ticker.Items.Enqueue(new MarketDisplayItem { }.setDisplaySettings("Void Opals", "906,045 cr", "1.2% ▲"));
        }

        private void ticker_ItemDisplayed(object sender, ItemEventArgs<MarketDisplayItem> e) {
            // "IR: " + ticker.IsRunning + " " + ticker.Items.Count;
            VM.VisibleItems = $"{(ticker.IsRunning ? 0 : 1)},{ticker.Items.Count}"; 
        }

        private void MetroWindow_MouseEnter(object sender, MouseEventArgs e) {
            ticker.Stop();
            VM.VisibleItems = $"{(ticker.IsRunning ? 0 : 1)},{ticker.Items.Count}";
        }

        private void MetroWindow_MouseLeave(object sender, MouseEventArgs e) {
            ticker.Start();
            VM.VisibleItems = $"{(ticker.IsRunning ? 0 : 1)},{ticker.Items.Count}";
        }
    }
}
