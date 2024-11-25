using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class DonHangViewModel : MainWindowTabViewModel
    {
        public DonHangViewModel()
        {
            IsLoadedComplete = true;
        }

        [RelayCommand]
        void TaoDonHang()
        {
            var taoDonHangWindow = new IT008_QuanLyBanHang.View.TaoDonHangView();
            taoDonHangWindow.Show();
        }
    }
}
