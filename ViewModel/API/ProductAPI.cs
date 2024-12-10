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
            List<Batch>? batches = await BatchAPI.GetAllBatches();
            if (products == null || batches == null)
                return new List<BatchProduct>();
            List<BatchProduct> batchProducts = new();
            foreach (Product p in products)
            {
                if (p.Batches != null)
                {
                    foreach (var b in p.Batches)
                    {
                        Batch? batch = batches.Find(ba => ba.Id == b);
                        BatchProduct bp;
                        if (batch != null)
                            bp = new(p, batch);
                        else
                            bp = new(p, new Batch());
                        batchProducts.Add(bp);
                    }
                }
            }
            return batchProducts;
        }

        static public async Task<List<Batch>> GetBatchesOfProduct(Product product)
        {
            List<Batch>? batches = await BatchAPI.GetAllBatches();
            if (batches == null)
                return new();

            List<Batch> productBatches = new();
            if (product.Batches != null)
            {
                foreach (var b in product.Batches)
                {
                    Batch? batch = batches.Find(ba => ba.Id == b);
                    if (batch != null)
                        productBatches.Add(batch);
                }
            }
            return productBatches;
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
