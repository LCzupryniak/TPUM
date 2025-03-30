using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Logic.Interfaces;

namespace Presentation.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IOrderService _orderService;
        private IInventoryManagement _inventoryService;
        private ShopApp.Presentation.Models.ShoppingCart _cart;

        public ObservableCollection<Product> AvailableProducts { get; private set; }
        public ObservableCollection<Order> CustomerOrders { get; private set; }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal CartTotal
        {
            get { return _cart.TotalPrice; }
        }

        public ICommand CreateOrderCommand { get; private set; }
        public ICommand AddToCartCommand { get; private set; }

        public MainViewModel(IOrderProcessing orderService, IInventoryManagement inventoryService)
        {
            _orderService = orderService;
            _inventoryService = inventoryService;
            _cart = new ShopApp.Presentation.Models.ShoppingCart();

            AvailableProducts = new ObservableCollection<Product>();
            CustomerOrders = new ObservableCollection<Order>();

            CreateOrderCommand = new RelayCommand(ExecuteCreateOrder, CanCreateOrder);
            AddToCartCommand = new RelayCommand(ExecuteAddToCart, CanAddToCart);

            // Inicjalizacja przykładowych danych
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            // Dodanie przykładowych produktów do listy
            AvailableProducts.Add(new Product { ProductId = 1, Name = "Laptop", Price = 3500.00m, Quantity = 10 });
            AvailableProducts.Add(new Product { ProductId = 2, Name = "Smartfon", Price = 1200.00m, Quantity = 15 });
            AvailableProducts.Add(new Product { ProductId = 3, Name = "Słuchawki", Price = 200.00m, Quantity = 20 });
        }

        private bool CanCreateOrder()
        {
            return _cart.Items.Count > 0;
        }

        private void ExecuteCreateOrder()
        {
            Customer customer = new Customer
            {
                CustomerId = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                ContactInfo = "jan.kowalski@example.com"
            };

            Order newOrder = _orderService.CreateNewOrder(customer, new List<Product>(_cart.Items));
            CustomerOrders.Add(newOrder);

            // Wyczyść koszyk po złożeniu zamówienia
            _cart.Items.Clear();
            _cart.TotalPrice = 0;
            OnPropertyChanged("CartTotal");
        }

        private bool CanAddToCart()
        {
            return SelectedProduct != null && SelectedProduct.IsAvailable();
        }

        private void ExecuteAddToCart()
        {
            if (SelectedProduct != null)
            {
                _cart.AddItem(SelectedProduct);
                OnPropertyChanged("CartTotal");
                _inventoryService.AdjustProductQuantity(SelectedProduct.ProductId, -1);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
