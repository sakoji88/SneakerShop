using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SneakerShop.Model
{
    [Table("Users")]
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Login { get; set; }

        [Required, StringLength(128)]
        public string Password { get; set; }

        [Required, StringLength(120)]
        public string Email { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }

    [Table("Clones")]
    public partial class Clone : INotifyPropertyChanged
    {
        public Clone()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int Intelligence { get; set; }
        public int Toxicity { get; set; }
        public int Charisma { get; set; }

        [StringLength(255)]
        public string ImagePath { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [Table("Orders")]
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalPrice { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    [Table("OrderItems")]
    public partial class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int CloneId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }

        [ForeignKey(nameof(CloneId))]
        public virtual Clone Clone { get; set; }
    }
}
