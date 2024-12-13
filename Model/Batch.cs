using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public class Batch
    {
        public int Id { get; set; }
        public string? BatchNumber { get; set; }
        public int Quantity { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public float ImportPrice { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime ManufactureDate { get; set; }
    }
}
