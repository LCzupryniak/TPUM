using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.API;
using Model;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IShopService _shopService;
        private readonly IProductStockNotifier _stockNotifier;

        public ObservableCollection<ProductViewModel> Products { get; set; }

        public PurchaseCommand PurchaseCommand { get; set; }
        public ExitCommand ExitCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel(IShopService shopService, IProductStockNotifier stockNotifier)
        {
            _shopService = shopService;
            _stockNotifier = stockNotifier;

            Products = new ObservableCollection<ProductViewModel>();

            PurchaseCommand = new PurchaseCommand(shopService, Products);
            ExitCommand = new ExitCommand();

            LoadProducts();

            _stockNotifier.StockChanged += OnStockChanged;
            _stockNotifier.StartMonitoring();
        }

        private void LoadProducts()
        {
            Products.Clear();
            foreach (var product in _shopService.GetAvailableProducts())
            {
                Products.Add(new ProductViewModel
                {
                    Name = product.Name,
                    Price = product.Price,
                    Stock = _stockNotifier.GetCurrentStock(product.Name)
                });
            }
        }

        private void OnStockChanged(object sender, EventArgs e)
        {
            foreach (var item in Products)
            {
                item.Stock = _stockNotifier.GetCurrentStock(item.Name);
            }

            OnPropertyChanged("Products");
        }

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
