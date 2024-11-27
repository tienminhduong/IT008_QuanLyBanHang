using CommunityToolkit.Mvvm.ComponentModel;
using IT008_QuanLyBanHang.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class KhachHangViewModel : ObservableObject, ITabViewModel
    {
        public KhachHangViewModel()
        {
        }

        public async Task LoadData()
        {
            await Task.Delay(1000);
        }
    }
}
