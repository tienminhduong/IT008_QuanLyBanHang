using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT008_QuanLyBanHang.Model
{
    public partial class BatchProduct : ObservableObject
    {
        public BatchProduct()
        {
            id = 0;
            productName = "";
            status = 0;
            category = new Category();
            batch = new Batch();
        }

        public BatchProduct(Product product, Batch batch)
        {
            id = product.Id;
            productName = product.ProductName;
            status = product.Status;
            category = product.Category;
            this.batch = batch;
        }

        [ObservableProperty]
        private int id;
        [ObservableProperty]
        private string? productName;
        [ObservableProperty]
        private int status;
        [ObservableProperty]
        private Category? category;
        [ObservableProperty]
        private Batch? batch;
    }
}
