using System.Windows;
using SneakerShop.ApplicationData;
using SneakerShop.Pages;

namespace SneakerShop
{
    /// <summary>
    /// Главное окно с Frame-навигацией.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppFrame.MainFrame = MainFrame;
            MainFrame.Navigate(new Authorization());
        }
    }
}
