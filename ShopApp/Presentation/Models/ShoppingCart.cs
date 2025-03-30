using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Models
{
    internal class ShoppingCart : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _items;
        private decimal _totalPrice;

        public ObservableCollection<Product> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        public ShoppingCart()
        {
            _items = new ObservableCollection<Product>();
        }

        public void AddItem(Product product)
        {
            _items.Add(product);
            TotalPrice += product.Price;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
}
