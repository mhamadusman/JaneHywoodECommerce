using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CartItem
    {
        public string UserId { set; get; }

        public int ProductId { set; get; }
        public string ProductName { set; get; }
        public string ProductDescription { set; get; }

        public string Category { set; get; }
        public decimal ProductPrice { set; get; }
        public decimal DiscPrice { get; set; }

        public string Img { set; get; }

        public string ProductSize { set; get; }
        public int Quantity { get; set; }



    }
}
