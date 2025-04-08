using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Logic.API;
using ViewModel;



namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ILogicFactory factory = LogicFactory.CreateFactory();
            var shopService = factory.CreateShopService();
            var stockNotifier = factory.CreateStockNotifier();

            DataContext = new MainViewModel(shopService, stockNotifier);
        }
    }
}