using IT008_QuanLyBanHang.DTOs;
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

        public Batch(BatchDTO dto)
        {
            Id = dto.id;
            BatchNumber = dto.batch_number;
            Quantity = dto.quantity;
            Stock = dto.stock;
            Price = float.Parse(dto.price ?? "0");
            ImportPrice = float.Parse(dto.import_price ?? "0");
            ExpirationDate = dto.expiration_date;
            ManufactureDate = dto.manufacture_date;
        }

        public Batch()
        {
            
        }
    }
}
