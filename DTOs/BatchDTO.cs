using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.DTOs
{
    public class BatchDTO
    {
        public int id { get; set; }
        public string? batch_number { get; set; }
        public int quantity { get; set; }
        public int stock { get; set; }
        public string? price { get; set; }
        public string? import_price { get; set; }
        public DateTime expiration_date { get; set; }
        public DateTime manufacture_date { get; set; }
        public DateTime created_at { get; set; }

        public BatchDTO(Batch batch)
        {
            id = batch.Id;
            batch_number = batch.BatchNumber;
            quantity = batch.Quantity;
            stock = batch.Stock;
            price = batch.Price.ToString();
            import_price = batch.ImportPrice.ToString();
            expiration_date = batch.ExpirationDate;
            manufacture_date = batch.ManufactureDate;
            created_at = batch.CreatedAt;
        }
        public BatchDTO()
        {
            
        }
    }
}
