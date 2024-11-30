using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class DonHangViewModel : ObservableObject, ITabViewModel
    {
        public DonHangViewModel()
        {
        }

        public async Task LoadData()
        {
            await Task.Delay(1000);
        }

        [RelayCommand]
        void TaoDonHang()
        {
            var taoDonHangWindow = new IT008_QuanLyBanHang.View.TaoDonHangView();
            taoDonHangWindow.Show();
        }
    }
}
