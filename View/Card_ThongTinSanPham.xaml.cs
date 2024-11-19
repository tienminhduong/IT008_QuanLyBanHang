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

namespace IT008_QuanLyBanHang.View
{
    /// <summary>
    /// Interaction logic for Card_ThongTinSanPham.xaml
    /// </summary>
    public partial class Card_ThongTinSanPham : UserControl
    {
        public Card_ThongTinSanPham()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TextBlock_ProductName.Text = ProductName;
            TextBlock_ManufactureDate.Text = ManufactureDate.ToString("dd/MM/yyyy");
            TextBlock_ExpirationDate.Text = ExpirationDate.ToString("dd/MM/yyyy");
            TextBlock_Price.Text = Price;
            TextBlock_Quantity.Text = Quantity.ToString();
        }

        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }

        public DateTime ManufactureDate
        {
            get { return (DateTime)GetValue(ManufactureDateProperty); }
            set { SetValue(ManufactureDateProperty, value); }
        }

        public DateTime ExpirationDate
        {
            get { return (DateTime)GetValue(ExpirationDateProperty); }
            set { SetValue(ExpirationDateProperty, value); }
        }

        public string Price
        {
            get { return (string)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        public int Quantity
        {
            get { return (int)GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }

        public static readonly DependencyProperty ProductNameProperty = DependencyProperty.Register("ProductName", typeof(string), typeof(Card_ThongTinSanPham));
        public static readonly DependencyProperty ManufactureDateProperty = DependencyProperty.Register("ManufactureDate", typeof(DateTime), typeof(Card_ThongTinSanPham));
        public static readonly DependencyProperty ExpirationDateProperty = DependencyProperty.Register("ExpirationDate", typeof(DateTime), typeof(Card_ThongTinSanPham));
        public static readonly DependencyProperty PriceProperty = DependencyProperty.Register("Price", typeof(string), typeof(Card_ThongTinSanPham));
        public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register("Quantity", typeof(int), typeof(Card_ThongTinSanPham));
    }
}
