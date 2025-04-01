using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Interfaces;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class ProductCatalogViewModel : ViewModelBase
    {
        private readonly IProductService _productService;
        private ProductModel _selectedProduct;
        private List<ProductModel> _products;
        private string _searchQuery;
        private string _selectedCategory;
        private List<string> _categories;

        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public List<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged("Products");
            }
        }

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged("SearchQuery");
                FilterProducts();
            }
        }

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
                FilterProducts();
            }
        }

        public List<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged("Categories");
            }
        }

        public ProductCatalogViewModel(IProductService productService)
        {
            _productService = productService;
            _products = new List<ProductModel>();
            _categories = new List<string>();
            LoadProducts();
            InitializeCategories();
        }

        private void LoadProducts()
        {
            // W rzeczywistej aplikacji tutaj pobieralibyśmy dane z serwisu
            var productList = new List<ProductModel>
        {
            new ProductModel
            {
                Id = 1,
                Name = "Telewizor LED 55\"",
                Description = "Telewizor o wysokiej rozdzielczości",
                Price = 2499.99m,
                StockQuantity = 10,
                Category = "Elektronika"
            },
            new ProductModel
            {
                Id = 2,
                Name = "Koszulka bawełniana",
                Description = "Koszulka z czystej bawełny",
                Price = 49.99m,
                StockQuantity = 100,
                Category = "Odzież"
            },
            new ProductModel
            {
                Id = 3,
                Name = "Programowanie w C#",
                Description = "Książka dla początkujących programistów",
                Price = 89.99m,
                StockQuantity = 25,
                Category = "Książki"
            }
        };

            Products = productList;
            SelectedProduct = Products.FirstOrDefault();
        }

        private void InitializeCategories()
        {
            Categories = Products.Select(p => p.Category).Distinct().ToList();
            Categories.Insert(0, "Wszystkie kategorie");
            SelectedCategory = Categories[0];
        }

        private void FilterProducts()
        {
            // Implementacja filtrowania produktów
            // W rzeczywistej aplikacji użylibyśmy tu danych z serwisu
        }

        public void AddToCart(int productId, int quantity)
        {
            // Implementacja dodawania do koszyka
            // W rzeczywistej aplikacji wywoływalibyśmy tu metodę serwisu
        }
    }
}
