using System;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Models;

public class Product
{

    public int Id { get; set;}

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    public string? Description { get; set; }

    public DateTime  CreatedAt { get; set;} = DateTime.UtcNow;

    public string? CreatedBy { get; set; }
    
}
