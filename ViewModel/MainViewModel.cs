using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IShopModelService _shopService;
        private readonly IProductStockModelNotifier _stockNotifier;

        public ObservableCollection<ProductViewModel> Products { get; set; }

        public PurchaseCommand PurchaseCommand { get; set; }
        public ExitCommand ExitCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel(IShopModelService shopService, IProductStockModelNotifier stockNotifier)
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
                    Price = (double)product.Price,
                    Stock = _stockNotifier.GetCurrentStock(product.Name)
                });
            }
        }

        private void OnStockChanged(object sender, System.EventArgs e)
        {
            foreach (var item in Products)
            {
                item.Stock = _stockNotifier.GetCurrentStock(item.Name);
            }

            OnPropertyChanged("Products");
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
