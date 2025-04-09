using System.Windows;
using Model;
using ViewModel;
using Logic.API;

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
    }
}
