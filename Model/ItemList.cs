using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public partial class ItemList : ObservableObject
    {
        [ObservableProperty]
        List<BatchProduct> batchProducts = new();
    }
}
