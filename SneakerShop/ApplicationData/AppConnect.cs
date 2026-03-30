using SneakerShop.Model;

namespace SneakerShop.ApplicationData
{
    /// <summary>
    /// Статический доступ к контексту БД (как в учебных проектах).
    /// </summary>
    public static class AppConnect
    {
        static AppConnect()
        {
            // При старте гарантируем наличие таблиц/данных в CloneShop.
            DbBootstrapper.EnsureDatabaseAndSchema();
        }

        public static CloneShopEntities Modeldb { get; } = new CloneShopEntities();
    }
}
