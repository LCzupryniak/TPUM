using Client.Logic.API;
using Client.Presentation.Model.API;
using Client.Presentation.ViewModel.MVVMLight;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Client.Presentation.ViewModel
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        #region Fields

        // Services
        private readonly ICustomerModelService _customerService;
        private readonly IProductModelService _itemService;
        private readonly IOrderModelService _orderService;
        private readonly SynchronizationContext? _syncContext;

        private ICustomerModel? _selectedCustomer;
        private IProductModel? _selectedShopItem;
        private float _selectedCustomerMoney;

        #endregion

        #region Properties

        public ObservableCollection<ICustomerModel> Customers { get; } = new ObservableCollection<ICustomerModel>();
        public ObservableCollection<IProductModel> SelectedCustomerCart { get; } = new ObservableCollection<IProductModel>();
        public ObservableCollection<IProductModel> ShopItems { get; } = new ObservableCollection<IProductModel>();
        public ObservableCollection<IOrderModel> Orders { get; } = new ObservableCollection<IOrderModel>();

        // Selected Customer
        public ICustomerModel? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (SetField(ref _selectedCustomer, value))
                {
                    UpdateSelectedCustomerUIData();
                    BuyItemCommand.RaiseCanExecuteChanged(); // Update connected commands
                }
            }
        }

        // Selected Item
        public IProductModel? SelectedShopItem
        {
            get => _selectedShopItem;
            set
            {
                if (SetField(ref _selectedShopItem, value))
                {
                    BuyItemCommand.RaiseCanExecuteChanged(); // Update connected commands
                }
            }
        }

        public float SelectedCustomerMoney
        {
            get => _selectedCustomerMoney;
            private set => SetField(ref _selectedCustomerMoney, value);
        }

        private bool _isConnected = false;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (SetField(ref _isConnected, value))
                {
                    ConnectToServerCommand.RaiseCanExecuteChanged();
                    DisconnectFromServerCommand.RaiseCanExecuteChanged();
                }
            }
        }


        // Commands
        public RelayCommand BuyItemCommand { get; }
        public RelayCommand ConnectToServerCommand { get; }
        public RelayCommand DisconnectFromServerCommand { get; }

        #endregion

        #region Constructors

        public MainViewModel()
            // create default services from the factory and inject them to constructor below
            : this(ModelFactory.CreateCustomerModelService(),
                   ModelFactory.CreateItemModelService(),
                   ModelFactory.CreateOrderModelService())
        {
        }

        public IConnectionService _connectionService = null!;

        public MainViewModel(
            ICustomerModelService customerService,
            IProductModelService itemService,
            IOrderModelService orderService)
        {
            // Initialize Commands
            BuyItemCommand = new RelayCommand(ExecuteBuyItem, CanExecuteBuyItem);
            ConnectToServerCommand = new RelayCommand(ExecuteConnectToServer, CanConnectToServer);
            DisconnectFromServerCommand = new RelayCommand(ExecuteDisconnectFromServer, CanDisconnectFromServer);

            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

            _connectionService = LogicFactory.CreateConnectionService(null, async () =>
            {
                _ = LoadInitialDataAsync();
            });

            Task.Run(async () =>
            {
                IsConnected = true;
                await _connectionService.Connect(new Uri("ws://localhost:9081/ws"));

                // run after 2s to give time to connect
                await Task.Delay(2000).ContinueWith(async _ =>
                {
                    await _connectionService.FetchCustomers();
                    await _connectionService.FetchItems();
                    await _connectionService.FetchCarts();
                });
            });

            _syncContext = SynchronizationContext.Current;
            if (_syncContext == null)
            {
                Debug.WriteLine("Warning: MainViewModel created outside of WPF project");
            }

            Debug.WriteLine("MainViewModel creation complete.");
        }

        #endregion

        #region Data Loading

        private async Task LoadInitialDataAsync()
        {
            await Task.WhenAll(
                LoadCustomersAsync(),
                LoadShopItemsAsync(),
                RefreshOrdersAsync()
            );
            Debug.WriteLine("Initial data loading complete.");
        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                List<ICustomerModel> customers = await Task.Run(() => _customerService.GetAllCustomers().ToList());
                UpdateObservableCollection(Customers, customers);

                if (customers.Any())
                {
                    Action selectFirstCustomer = () => SelectedCustomer = customers[0];
                    if (_syncContext != null)
                    {
                        _syncContext.Post(_ => selectFirstCustomer(), null);
                    }
                    else
                    {
                        selectFirstCustomer();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("loading customers", ex);
            }
        }

        private async Task LoadShopItemsAsync()
        {
            try
            {
                List<IProductModel> items = await Task.Run(() => _itemService.GetAllItems().ToList());
                UpdateObservableCollection(ShopItems, items);
            }
            catch (Exception ex) { LogError("loading shop items", ex); }
        }

        public async Task RefreshOrdersAsync()
        {
            try
            {
                List<IOrderModel> orders = await Task.Run(() => _orderService.GetAllOrders().ToList());
                UpdateObservableCollection(Orders, orders);
            }
            catch (Exception ex) { LogError("refreshing orders", ex); }
        }

        public void UpdateObservableCollection<T>(ObservableCollection<T> collection, List<T> newData)
        {
            Action updateAction = () =>
            {
                collection.Clear();
                foreach (T item in newData)
                {
                    collection.Add(item);
                }
            };

            if (_syncContext != null)
            {
                _syncContext.Post(_ => updateAction(), null);
            }
            else
            {
                updateAction();
            }
        }

        private void UpdateSelectedCustomerUIData()
        {
            if (_selectedCustomer != null)
            {
                SelectedCustomerMoney = _selectedCustomer.Money;
                SelectedCustomerCart.Clear();
                foreach (IProductModel item in _selectedCustomer.Cart.Items)
                {
                    SelectedCustomerCart.Add(item);
                }
            }
            else
            {
                SelectedCustomerMoney = 0;
                SelectedCustomerCart.Clear();
            }
        }

        #endregion

        #region Command Implementations

        private void ExecuteConnectToServer(object? parameter)
        {
            IsConnected = true;

            Task.Run(async () =>
            {
                await _connectionService.Connect(new Uri("ws://localhost:9081/ws"));
            });
        }

        private bool CanConnectToServer(object? parameter)
        {
            return !IsConnected;
        }

        private async void ExecuteDisconnectFromServer(object? parameter)
        {
            await _connectionService.Disconnect();

            IsConnected = false;
        }

        private bool CanDisconnectFromServer(object? parameter)
        {
            return IsConnected;
        }

        private bool CanExecuteBuyItem(object? parameter)
        {
            // Can only buy if a customer and item are selected
            return SelectedCustomer != null && SelectedShopItem != null;
        }

        private async void ExecuteBuyItem(object? parameter)
        {
            ICustomerModel? buyer = SelectedCustomer;
            IProductModel? itemToBuy = SelectedShopItem;
            if (buyer == null || itemToBuy == null)
            {
                Debug.WriteLine($"Cannot buy item, buyer or item is null.");
                return;
            }

            Debug.WriteLine($"Attempting purchase for {buyer.Name}, Item {itemToBuy.Name}");

            await _connectionService.CreateOrder(Guid.NewGuid(), buyer.Id, [itemToBuy.Id]);
        }

        #endregion


        #region Utility & Cleanup

        private ICustomerModel? GetSelectedCustomerForMaintenance() => _selectedCustomer;

        private void LogError(string action, Exception ex)
        {
            Debug.WriteLine($"Error during {action}: {ex.Message}{Environment.NewLine}StackTrace: {ex.StackTrace}");
        }

        public void Dispose()
        {
            if (IsConnected)
                _connectionService.Disconnect().Wait();

            Debug.WriteLine("Disposing MainViewModel...");
            GC.SuppressFinalize(this);
            Debug.WriteLine("MainViewModel disposed.");
        }

        #endregion
    }
}