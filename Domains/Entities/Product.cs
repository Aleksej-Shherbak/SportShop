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
        
        public string Name { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        public string Category { get; set; }
        
        public decimal Price { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [ScaffoldColumn(false)]
        public DateTime CreatedAt { get; set; }
    }
}