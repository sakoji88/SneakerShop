using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SneakerShop.ApplicationData;

namespace SneakerShop.Pages
{
    /// <summary>
    /// Страница входа в систему.
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTb.Text.Trim();
            var password = PasswordPb.Password.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль.", "Проверка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = AppConnect.Modeldb.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UserSession.CurrentUser = user;
            AppFrame.MainFrame.Navigate(new MainPage());
        }

        private void GoRegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new Registration());
        }
    }
}
