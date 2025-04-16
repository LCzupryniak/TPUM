using System.Windows;
using Model;
using ViewModel;
using Logic.API;
using System.Text.RegularExpressions;
using Client.Data;

namespace View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ILogicFactory factory = LogicFactory.CreateFactory();
            var shopService = factory.CreateShopService();
            var stockNotifier = factory.CreateStockNotifier();


            var logicModelBridge = new LogicModelBridge(shopService, stockNotifier);

            DataContext = new MainViewModel(logicModelBridge, logicModelBridge);
        }
        private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var context = ((FrameworkElement)sender).DataContext as ProductViewModel;
            if (context != null)
            {
                context.SelectedQuantity++;
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var context = ((FrameworkElement)sender).DataContext as ProductViewModel;
            if (context != null && context.SelectedQuantity > 0)
            {
                context.SelectedQuantity--;
            }
        }
        //private async void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var socketClient = new WebSocketClient();
        //    await socketClient.ConnectAsync("ws://localhost:8080/ws");

        //    var shopService = new ShopModelService();
        //    var stockNotifier = new ProductStockNotifier();

        //    DataContext = new MainViewModel(shopService, stockNotifier, socketClient);
        //}
    }
}
