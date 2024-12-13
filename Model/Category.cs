using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
    }
}