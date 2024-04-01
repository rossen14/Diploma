using System.Collections.Generic;

namespace Diploma.Models
{
    public class Product
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
