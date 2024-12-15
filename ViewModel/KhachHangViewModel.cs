using CommunityToolkit.Mvvm.ComponentModel;
using IT008_QuanLyBanHang.DTOs;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class KhachHangViewModel : ObservableObject, ITabViewModel
    {
        public KhachHangViewModel()
        {
            Task.Run(() => LoadData());
        }

        [ObservableProperty]
        ObservableCollection<Customer> customers = new();

        List<Customer> customersList = new();
        List<Order> ordersList = new();

        [ObservableProperty] DataGridRowDetailsVisibilityMode showDetails = DataGridRowDetailsVisibilityMode.Collapsed;

        public async Task LoadData()
        {
            customersList = await CustomerAPI.GetAllCustomers();
            ordersList = await OrderAPI.GetAllOrders();

            foreach (Customer customer in customersList)
                customer.UpdateOrderList(ordersList);

            Customers = new ObservableCollection<Customer>(customersList);
        }
    }
}
