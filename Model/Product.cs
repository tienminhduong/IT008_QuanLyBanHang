using CommunityToolkit.Mvvm.ComponentModel;
using IT008_QuanLyBanHang.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public partial class Product : ObservableObject
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? Status { get; set; }
        public string? ImageURL { get; set; }
        public string? Unit { get; set; }
        public Category? Category { get; set; }
        public List<Batch> Batches { get; set; }
        public int Price { get; set; }
        public int ImportPrice { get; set; }
        public int Stock { get; set; }
        public int Quantity { get; set; }

        public Product(ProductDTO dto, List<Batch> batches, List<Category> categories)
        {
            Id = dto.id;
            ProductName = dto.product_name;
            Status = dto.status;
            ImageURL = dto.image_url;
            Unit = dto.unit;
            Category = categories.FirstOrDefault(c => c.Id == dto.category_id);
            Batches = batches.Where(b => dto.batch_ids?.Contains(b.Id) == true).ToList();
            Price = Batches.Count > 0 ? (int)Batches.Average(b => b.Price) : 0;
            ImportPrice = Batches.Count > 0 ? (int)Batches.Average(b => b.ImportPrice) : 0;
            Stock = Batches.Sum(b => b.Stock);
            Quantity = Batches.Sum(b => b.Quantity);
        }
    }
}
