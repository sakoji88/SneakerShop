using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SneakerShop.ApplicationData;

namespace SneakerShop.Pages
{
    /// <summary>
    /// Страница просмотра заказов пользователя.
    /// </summary>
    public partial class Orders : Page
    {
        public Orders()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            if (UserSession.CurrentUser == null)
            {
                OrdersDg.ItemsSource = null;
                return;
            }

            var orders = AppConnect.Modeldb.Orders
                .Where(o => o.UserId == UserSession.CurrentUser.Id)
                .Select(o => new
                {
                    o.Id,
                    o.Date,
                    o.TotalPrice,
                    ItemCount = o.OrderItems.Sum(i => i.Quantity)
                })
                .OrderByDescending(o => o.Date)
                .ToList();

            OrdersDg.ItemsSource = orders;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new MainPage());
        }
    }
}
