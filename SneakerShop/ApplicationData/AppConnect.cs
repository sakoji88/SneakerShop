using SneakerShop.Model;

namespace SneakerShop.ApplicationData
{
    /// <summary>
    /// Статический доступ к контексту БД (как в учебных проектах).
    /// </summary>
    public static class AppConnect
    {
        public static CloneShopEntities Modeldb { get; } = new CloneShopEntities();
    }
}
