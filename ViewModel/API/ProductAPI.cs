using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel.API
{
    public static class ProductAPI
    {
        static public async Task<List<Product>?> GetAllProducts()
        {
            string resStr = await RESTService.Instance.GetAsyncOfType("products");
            Response? res = JsonSerializer.Deserialize<Response>(resStr);
            if (res == null || res.Data == null)
                return null;
            return res.Data.Items;
        }

        static public async Task<List<BatchProduct>> GetAllProductsWithBatches()
        {
            List<Product>? products = await GetAllProducts();
            if (products == null)
                return new List<BatchProduct>();
            List<BatchProduct> batchProducts = new();
            foreach (Product p in products)
            {
                if (p.Batches != null)
                {
                    foreach (var b in p.Batches)
                    {
                        BatchProduct bp = new(p, b);
                        batchProducts.Add(bp);
                    }
                }
            }
            return batchProducts;
        }

        public class Response
        {
            [JsonPropertyName("data")]
            public Data? Data { get; set; }
        }

        public class Data
        {
            [JsonPropertyName("items")]
            public List<Product>? Items { get; set; }
        }
    }
}
