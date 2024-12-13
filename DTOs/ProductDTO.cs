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
    }
}
