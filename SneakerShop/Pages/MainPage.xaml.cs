using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SneakerShop.ApplicationData;
using SneakerShop.Model;
using SneakerShop.Services;

namespace SneakerShop.Pages
{
    /// <summary>
    /// Главная страница каталога клонов.
    /// </summary>
    public partial class MainPage : Page
    {
        private List<Clone> _allClones = new List<Clone>();

        public MainPage()
        {
            InitializeComponent();
            InitFilters();
            LoadData();
        }

        private void InitFilters()
        {
            ToxicityCb.ItemsSource = new List<string>
            {
                "Любая токсичность",
                "0-30",
                "31-60",
                "61-100"
            };
            ToxicityCb.SelectedIndex = 0;

            SortCb.ItemsSource = new List<string>
            {
                "Цена: по возрастанию",
                "Цена: по убыванию"
            };
            SortCb.SelectedIndex = 0;
        }

        private void LoadData()
        {
            _allClones = AppConnect.Modeldb.Clones.ToList();
            ApplyFilterSort();
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            ApplyFilterSort();
        }

        private void ApplyFilterSort()
        {
            var filtered = _allClones.AsEnumerable();

            var searchText = SearchTb.Text?.Trim().ToLower() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(c => c.Name.ToLower().Contains(searchText));
            }

            switch (ToxicityCb.SelectedIndex)
            {
                case 1:
                    filtered = filtered.Where(c => c.Toxicity <= 30);
                    break;
                case 2:
                    filtered = filtered.Where(c => c.Toxicity >= 31 && c.Toxicity <= 60);
                    break;
                case 3:
                    filtered = filtered.Where(c => c.Toxicity >= 61);
                    break;
            }

            filtered = SortCb.SelectedIndex == 0
                ? filtered.OrderBy(c => c.Price)
                : filtered.OrderByDescending(c => c.Price);

            ClonesLv.ItemsSource = filtered.ToList();
        }

        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            var clone = (sender as Button)?.Tag as Clone;
            if (clone == null || UserSession.CurrentUser == null)
            {
                MessageBox.Show("Не удалось оформить покупку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var order = new Order
            {
                UserId = UserSession.CurrentUser.Id,
                Date = DateTime.Now,
                TotalPrice = clone.Price
            };

            AppConnect.Modeldb.Orders.Add(order);
            AppConnect.Modeldb.SaveChanges();

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                CloneId = clone.Id,
                Quantity = 1
            };

            AppConnect.Modeldb.OrderItems.Add(orderItem);
            AppConnect.Modeldb.SaveChanges();

            var receiptPath = ReceiptService.GenerateReceipt(order, clone);
            MessageBox.Show($"Покупка успешна!\nID заказа: {order.Id}\nЧек: {receiptPath}",
                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new Orders());
        }
    }
}
