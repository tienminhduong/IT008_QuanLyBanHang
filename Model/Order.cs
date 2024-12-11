using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public class Order
    {
        public int Id { get; set; }
        public float TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Customer? Customer { get; set; }

    }
    public class OrderItem
    {

    }
}
