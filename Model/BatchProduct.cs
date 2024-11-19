using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.Model
{
    public partial class BatchProduct : ObservableObject
    {
        public BatchProduct(Product product, Batch batch)
        {
            this.product = product;
            this.batch = batch;
            visibility = Visibility.Visible;
        }

        [ObservableProperty]
        private Product product;

        [ObservableProperty]
        private Batch batch;

        [ObservableProperty]
        private Visibility visibility;

        [ObservableProperty]
        private bool isSelected;
    }
}
