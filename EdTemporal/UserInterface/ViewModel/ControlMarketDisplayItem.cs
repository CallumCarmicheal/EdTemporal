using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.UserInterface.ViewModel {
    public class ControlMarketDisplayItem : ViewModelBase {


        protected string _CommodityName = "Low Temperture Diamonds";
        public string CommodityName {
            get { return _CommodityName; }
            set {
                if (value != null || value != _CommodityName) _CommodityName = value;
                OnPropertyChanged("CommodityName");
            }
        }


        protected string _Value = "1,005,014 cr";
        public string Value {
            get { return _Value; }
            set {
                if (value != null || value != _Value) _Value = value;
                OnPropertyChanged("Value");
            }
        }

        protected string _PercentageChange = "6.2% ▲";
        public string PercentageChange {
            get { return _PercentageChange; }
            set {
                if (value != null || value != _PercentageChange) _PercentageChange = value;
                OnPropertyChanged("PercentageChange");
            }
        }
    }
}
