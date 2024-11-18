using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("product_name")]
        public string? ProductName { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("category")]
        public Category? Category { get; set; }
    }

    public class ProductData
    {
        [JsonPropertyName("items")]
        public List<Product>? Items { get; set; }
    }

    public class ProductResponse
    {
        [JsonPropertyName("data")]
        public ProductData? Data { get; set; }
    }
}
