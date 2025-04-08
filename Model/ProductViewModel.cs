using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public double Price { get; set; }

        private int _stock;
        public int Stock
        {
            get { return _stock; }
            set
            {
                _stock = value;
                OnPropertyChanged("Stock");
            }
        }

        private int _selectedQuantity;
        public int SelectedQuantity
        {
            get { return _selectedQuantity; }
            set
            {
                _selectedQuantity = value;
                OnPropertyChanged("SelectedQuantity");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
