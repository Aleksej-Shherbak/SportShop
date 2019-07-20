using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Entities
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Please enter a description of the product")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Please enter a price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a correct price value")]
        public decimal Price { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }
    }
}