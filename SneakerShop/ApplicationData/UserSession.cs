using SneakerShop.Model;

namespace SneakerShop.ApplicationData
{
    /// <summary>
    /// Текущий авторизованный пользователь.
    /// </summary>
    public static class UserSession
    {
        public static User CurrentUser { get; set; }
    }
}
