using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class TongQuanViewModel : ObservableObject, ITabViewModel
    {
        public TongQuanViewModel()
        {
            Task.Run(() => LoadData());
        }

        public async Task LoadData()
        {
            batchProducts = await ProductAPI.GetAllProductsWithBatches();
            orders = await OrderAPI.GetAllOrders();

            foreach (Order order in orders)
            {
                if (order.OrderDate.Date == DateTime.Today)
                    NumberSoldToday++;
            }

            foreach (Order order in orders)
            {
                if (order.OrderDate.Date == DateTime.Today)
                {
                    float tienVao = order.TotalAmount;
                    float tienRa = 0;
                    if (order.OrderItems != null)
                        foreach (OrderItem item in order.OrderItems)
                            tienRa += item.Batch?.ImportPrice ?? 0 * item.Quantity;

                    DoanhThuHomNay += (tienVao - tienRa);
                }
            }

            RecentOrders = new();
            var temp = orders.OrderByDescending(orders => orders.OrderDate);
            int count = 0;
            foreach (Order order in temp)
            {
                RecentOrders.Add(order);
                count++;
                if (count == 5)
                    break;
            }

            AboutToExpire = new();
            foreach (BatchProduct batchProduct in batchProducts)
            {
                Trace.WriteLine(batchProduct.Batch.ExpirationDate.Date.ToString("d"));
                if (batchProduct.Batch.ExpirationDate.Date <= DateTime.Today.AddDays(7)
                    && batchProduct.Batch.ExpirationDate.Date >= DateTime.Today)
                    AboutToExpire.Add(batchProduct);
            }
        }

        List<BatchProduct> batchProducts = new();
        List<Order> orders = new List<Order>();

        [ObservableProperty] int numberSoldToday = 0;
        [ObservableProperty] float doanhThuHomNay = 0;
        [ObservableProperty] ObservableCollection<Order> recentOrders = new();
        [ObservableProperty] ObservableCollection<BatchProduct> aboutToExpire = new();
    }
}
