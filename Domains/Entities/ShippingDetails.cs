using System.ComponentModel.DataAnnotations;

namespace Domains.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        public string City { get; set; }

        public string Zip { get; set; }

        [Required(ErrorMessage = "Please enter country name")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}