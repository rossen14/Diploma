using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Diploma.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderedOn { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
  
        public virtual Product Product { get; set; }
       
        public int Quantity { get; set; }
    }
}
