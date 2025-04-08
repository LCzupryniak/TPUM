using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.API;
using System.Windows.Input;
using Model;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public class PurchaseCommand : ICommand
    {
        private readonly IShopService _shopService;
        private readonly ObservableCollection<ProductViewModel> _products;

        public PurchaseCommand(IShopService shopService, ObservableCollection<ProductViewModel> products)
        {
            _shopService = shopService;
            _products = products;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            foreach (var product in _products)
            {
                if (product.SelectedQuantity > 0)
                {
                    for (int i = 0; i < product.SelectedQuantity; i++)
                    {
                        _shopService.PurchaseProduct(product.Name);
                        product.Stock--; // lokalnie aktualizujemy
                    }

                    product.SelectedQuantity = 0;
                }
            }
        }
    }
}
