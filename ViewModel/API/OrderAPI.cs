using IT008_QuanLyBanHang.DTOs;
using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel.API
{
    public class OrderAPI : BaseAPI<Order, OrderDTO>
    {
        public static async Task<List<Order>> GetAllOrders()
        {
            var dtos = await GetAllItemsDTO();
            List<Batch> batches = await BatchAPI.GetAllBatches();
            List<Category> categories = await CategoryAPI.GetAllCategories();

            List<Order> orders = new();
            Trace.WriteLine("From OrderAPI");
            foreach (var dto in dtos)
                orders.Add(new Order(dto, batches, categories));

            //Print out all orderList
            foreach (var order in orders)
                Trace.WriteLine($"Order ID: {order.Id}, {order.Customer?.FullName}, {order.Status}");

            return orders;
        }
    }
}
