using System;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthDemoNew.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    [Required]
    public string Category { get; set; } = null!;

    public int Stock { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
