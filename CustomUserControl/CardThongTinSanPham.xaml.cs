using IT008_QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IT008_QuanLyBanHang.CustomUserControl
{
    /// <summary>
    /// Interaction logic for CardThongTinSanPham.xaml
    /// </summary>
    public partial class CardThongTinSanPham : UserControl
    {
        public CardThongTinSanPham()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TextBlock_ID.Text = ProductBatch.Product.Id.ToString();
            TextBox_Quantity.Text = ProductBatch.Batch.Quantity.ToString();
            TextBlock_ProductName.Text = ProductBatch.Product.ProductName;
            TextBlock_CategoryName.Text = ProductBatch.Product.Category?.CategoryName;
            TextBlock_Price.Text = ProductBatch.Batch.Price.ToString();
            TextBlock_ManufactureDate.Text = ProductBatch.Batch.ManufactureDate.ToString("d");
            TextBlock_ExpirationDate.Text = ProductBatch.Batch.ExpirationDate.ToString("d");
            TextBlock_Unit.Text = ProductBatch.Product.Unit;
        }

        public BatchProduct ProductBatch
        {
            get { return (BatchProduct)GetValue(ProductBatchProperty); }
            set { SetValue(ProductBatchProperty, value); }
        }

        public static readonly DependencyProperty ProductBatchProperty = DependencyProperty.Register("ProductBatch", typeof(BatchProduct), typeof(CardThongTinSanPham));
    }
}
