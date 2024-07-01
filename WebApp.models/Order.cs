﻿using System.ComponentModel.DataAnnotations;

namespace WebApp.models
{

public class Order
{
    [Key]
    public Guid OrderId { get; set; }

    [Required]
    public string UserId { get; set; }

    public User User { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
}