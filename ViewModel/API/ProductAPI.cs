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

            List<Category>? categoriyList = await CategoryAPI.GetAllCategories();
            List<Product> resultList = new();

            foreach (ProductDTO dto in dtos)
            {
                Product? product = ConvertFromDTO(dto, batchList, categoriyList);
                if (product != null)
                    resultList.Add(product);
            }
            Trace.WriteLine("Product List returned by ProductAPI:");
            foreach (Product product in resultList)
                Trace.WriteLine($"Product: {product.Id}, {product.ProductName}, {product.Batches.Count}, {product.Category?.CategoryName}");

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

        static public async Task<List<Order>> GetOrdersOfProduct(Product product)
        {
            List<Order> orders = await OrderAPI.GetAllOrders();
            if (orders == null || !orders.Any())
                return new();

            List<Order> productOrders = new();
            foreach (var order in orders)
            {
                if (order.OrderItems != null &&
                    order.OrderItems.Any(item => item.Product?.Id == product.Id))
                {
                    productOrders.Add(order);
                }
            }

            Trace.WriteLine($"Orders containing product: {product.ProductName} (ID: {product.Id})");
            foreach (var order in productOrders)
            {
                Trace.WriteLine($"Order ID: {order.Id}, Customer: {order.Customer?.FullName}, Date: {order.OrderDate}, Status: {order.Status}");
                foreach (var item in order.OrderItems)
                {
                    Trace.WriteLine($"Product ID: {item.Product.Id}, Quanity: {item.Quantity}, Price: {item.Price}");
                }    
            }

            return productOrders;
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
