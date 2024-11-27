using CommunityToolkit.Mvvm.ComponentModel;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class TaoDonHangViewModel : ObservableObject, ITabViewModel
    {
        public TaoDonHangViewModel()
        {
        }

        public async Task LoadData()
        {
            await Task.Delay(1);
        }
    }
}
