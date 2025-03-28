using System;
using System.Collections.Generic;

namespace ShopApp.Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _productStorage;
        private object _lockObject;

        public ProductRepository()
        {
            _productStorage = new List<Product>();
            _lockObject = new object();
        }

        public void AddProduct(Product product)
        {
            lock (_lockObject)
            {
                product.ProductId = _productStorage.Count + 1;
                _productStorage.Add(product);
            }
        }

        public Product GetProductById(int productId)
        {
            lock (_lockObject)
            {
                Product foundProduct = null;
                foreach (Product product in _productStorage)
                {
                    if (product.ProductId == productId)
                    {
                        foundProduct = product;
                        break;
                    }
                }
                return foundProduct;
            }
        }

        public List<Product> GetAllProducts()
        {
            lock (_lockObject)
            {
                List<Product> productsCopy = new List<Product>();
                foreach (Product product in _productStorage)
                {
                    productsCopy.Add(product);
                }
                return productsCopy;
            }
        }

        public void UpdateProduct(Product product)
        {
            lock (_lockObject)
            {
                for (int i = 0; i < _productStorage.Count; i++)
                {
                    if (_productStorage[i].ProductId == product.ProductId)
                    {
                        _productStorage[i] = product;
                        break;
                    }
                }
            }
        }

        public void RemoveProduct(int productId)
        {
            lock (_lockObject)
            {
                for (int i = 0; i < _productStorage.Count; i++)
                {
                    if (_productStorage[i].ProductId == productId)
                    {
                        _productStorage.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }