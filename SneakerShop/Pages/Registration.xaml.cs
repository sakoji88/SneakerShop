using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SneakerShop.ApplicationData;
using SneakerShop.Model;

namespace SneakerShop.Pages
{
    /// <summary>
    /// Страница регистрации нового пользователя.
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = NameTb.Text.Trim();
            var login = LoginTb.Text.Trim();
            var email = EmailTb.Text.Trim();
            var password = PasswordPb.Password.Trim();
            var repeat = RepeatPasswordPb.Password.Trim();

            if (new[] { name, login, email, password, repeat }.Any(string.IsNullOrWhiteSpace))
            {
                MessageBox.Show("Заполните все поля.", "Проверка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != repeat)
            {
                MessageBox.Show("Пароли не совпадают.", "Проверка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (AppConnect.Modeldb.Users.Any(u => u.Login == login))
            {
                MessageBox.Show("Логин уже занят.", "Проверка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = new User
            {
                Name = name,
                Login = login,
                Email = email,
                Password = password
            };

            AppConnect.Modeldb.Users.Add(user);
            AppConnect.Modeldb.SaveChanges();

            MessageBox.Show("Регистрация успешна!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            AppFrame.MainFrame.Navigate(new Authorization());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new Authorization());
        }
    }
}
