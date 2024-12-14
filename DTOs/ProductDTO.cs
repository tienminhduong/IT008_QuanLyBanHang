using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.DTOs
{
    public class ProductDTO
    {
        public int id { get; set; }
        public string? product_name { get; set; }
        public string? status { get; set; }
        public string? image_url { get; set; }
        public string? unit { get; set; }
        public int category_id { get; set; }
        public List<int>? batch_ids { get; set; }
        public ProductDTO(Product product)
        {
            id = product.Id;
            product_name = product.ProductName;
            status = product.Status;
            image_url = product.ImageURL;
            unit = product.Unit;
            if (product.Category != null)
                category_id = product.Category.Id;
            foreach (Batch batch in product.Batches)
            {
                batch_ids ??= new List<int>();
                batch_ids.Add(batch.Id);
            }
        }

        public ProductDTO()
        {
            
        }
    }
}
