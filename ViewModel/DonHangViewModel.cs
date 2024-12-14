using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class DonHangViewModel : ObservableObject, ITabViewModel
    {
        public DonHangViewModel()
        {
            Task.Run(() => LoadData());
        }

        public async Task LoadData()
        {
            orders = await OrderAPI.GetAllOrders();
            OrderShown = new(orders);
        }

        [ObservableProperty] ObservableCollection<Order> orderShown = new();
        List<Order> orders = new();

        [RelayCommand]
        void TaoDonHang()
        {
            var taoDonHangWindow = new IT008_QuanLyBanHang.View.TaoDonHangView();
            taoDonHangWindow.Show();
        }
    }
}
