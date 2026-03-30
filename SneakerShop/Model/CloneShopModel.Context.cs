using System.Data.Entity;

namespace SneakerShop.Model
{
    /// <summary>
    /// Контекст Entity Framework (Database First-стиль).
    /// </summary>
    public partial class CloneShopEntities : DbContext
    {
        public CloneShopEntities()
            : base("name=CloneShopEntities")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Clone> Clones { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithRequired(o => o.User)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithRequired(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Clone>()
                .HasMany(c => c.OrderItems)
                .WithRequired(oi => oi.Clone)
                .HasForeignKey(oi => oi.CloneId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
