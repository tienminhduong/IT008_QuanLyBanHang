using IT008_QuanLyBanHang.DTOs;
using IT008_QuanLyBanHang.Interfaces;
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
    public class ProductAPI : BaseAPI<Product, ProductDTO>
    {
        static public async Task<List<Product>> GetAllProducts()
        {
            List<ProductDTO> dtos = await GetAllItemsDTO();
            List<Batch>? batchList = await BatchAPI.GetAllBatches();

            if (batchList == null)
                return new();

            List<Category>? categoriyList = new();
            List<Product> resultList = new();

            foreach (ProductDTO dto in dtos)
            {
                Product? product = ConvertFromDTO(dto, batchList, categoriyList);
                if (product != null)
                    resultList.Add(product);
            }
            Trace.WriteLine("Product List returned by ProductAPI:");
            foreach (Product product1 in resultList)
                Trace.WriteLine($"Product: {product1.Id}, {product1.ProductName}, {product1.Batches.Count}");

            return resultList;
        }

        static public async Task<List<BatchProduct>> GetAllProductsWithBatches()
        {
            List<Product> products = await GetAllProducts();

            List<BatchProduct> batchProducts = new();
            foreach (Product product in products)
            {
                if (product.Batches != null)
                {
                    foreach (Batch batch in product.Batches)
                    {
                        if (product.Batches.Contains(batch))
                            batchProducts.Add(new(product, batch));
                    }
                }
            }
            foreach (BatchProduct bp in batchProducts)
            {
                Trace.WriteLine($"BatchProduct: {bp.Product.ProductName}, {bp.Batch.BatchNumber}");
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
                    Batch? batch = batches.Find(ba => ba.Id == b.Id);
                    if (batch != null)
                        productBatches.Add(batch);
                }
            }
            return productBatches;
        }

        public static Product? ConvertFromDTO(ProductDTO dto, List<Batch> batches, List<Category> categories)
        {
            try
            {
                Product product = new(dto, batches, categories);
                return product;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error in ConvertFromDTO: {e.Message}");
                return null;
            }
        }
    }
}
