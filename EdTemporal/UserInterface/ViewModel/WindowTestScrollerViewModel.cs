using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdTemporal.UserInterface.ViewModel {
    public class WindowTestScrollerViewModel : ViewModelBase {


        protected string _VisibleItems;
        public string VisibleItems {
            get { return _VisibleItems; }
            set {
                if (value != _VisibleItems) _VisibleItems = value;
                OnPropertyChanged("VisibleItems");
            }
        }


    }
}
