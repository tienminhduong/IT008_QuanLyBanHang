using CommunityToolkit.Mvvm.ComponentModel;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class BaoCaoViewModel : ObservableObject, ITabViewModel
    {
        public BaoCaoViewModel()
        {
            YFormatter = value => value.ToString("C");
            XFormatter = value => SelectChartData.Contains("THÁNG") && (value == 0 || value == 9 || value == 19 || value == 29) ? (value + 1).ToString() : string.Empty;
            Task.Run(() => LoadData());
        }

        [ObservableProperty]
        private Func<double, string> yFormatter;

        [ObservableProperty]
        private Func<double, string> xFormatter;

        public async Task LoadData()
        {
            orderList = await OrderAPI.GetAllOrders();
            Orders.Clear();
            foreach (var order in orderList)
                Orders.Add(order);

            DoanhThuHomNay = TinhDoanhThuTheoNgay(DateTime.Today, Orders);
            DoanhThuHomQua = TinhDoanhThuTheoNgay(DateTime.Today.AddDays(-1), Orders);

            foreach (Order order in Orders)
            {
                if (order.OrderDate.Date == DateTime.Today)
                    SoHoaDonHomNay++;
            }
            foreach (Order order in Orders)
            {
                if (order.OrderDate.Date == DateTime.Today.AddDays(-1))
                    SoHoaDonHomQua++;
            }
        }

        public static float TinhDoanhThuTheoNgay(DateTime date, ObservableCollection<Order> orders)
        {
            float result = 0;
            foreach (Order order in orders)
            {
                if (order.OrderDate.Date == date)
                {
                    float tienVao = order.TotalAmount;
                    float tienRa = 0;
                    if (order.OrderItems != null)
                        foreach (OrderItem item in order.OrderItems)
                            tienRa += item.Batch?.ImportPrice ?? 0 * item.Quantity;

                    result += (tienVao - tienRa);
                }
            }
            Trace.WriteLine($"Calculating DoanhThu of date: {date.ToString()}, result is {result}, order count is {orders.Count}");
            return result;
        }

        List<Order> orderList = new();
        [ObservableProperty] ObservableCollection<Order> orders = new();

        [ObservableProperty] float soHoaDonHomNay = 0;
        [ObservableProperty] float soHoaDonHomQua = 0;
        [ObservableProperty] float doanhThuHomNay = 0;
        [ObservableProperty] float doanhThuHomQua = 0;
        [ObservableProperty] string selectChartData = "TUẦN NÀY";
    }

    public class StringToChartLabelsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string str)
                return new List<string>();
            if (str.Contains("TUẦN"))
                return new[] { "Hai", "Ba", "Tư", "Năm", "Sáu", "Bảy", "CN" };
            if (str.Contains("THÁNG"))
                return Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).Select(i => i.ToString()).ToArray();
            return new List<string>();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // write this again but with a different approach of using multi values converter using SelectedChartData and Orders
    public class ViewModelToChartSeriesCollectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return new SeriesCollection();
            if (values[0] is not string selectedChartData || values[1] is not ObservableCollection<Order> orders)
                return new SeriesCollection();
            if (selectedChartData.Contains("TUẦN"))
            {
                DateTime firstDayOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
                if (selectedChartData.Contains("TRƯỚC"))
                    firstDayOfWeek = firstDayOfWeek.AddDays(-7);
                // Get doanh thu of each day in the week
                List<float> doanhThu = new();
                for (int i = 0; i < 7; i++)
                {
                    DateTime date = firstDayOfWeek.AddDays(i);
                    //Trace.WriteLine($"Communicating from converter, doanh thu of {date.Date.ToString()}: {BaoCaoViewModel.TinhDoanhThuTheoNgay(date, orderList)}");
                    doanhThu.Add(BaoCaoViewModel.TinhDoanhThuTheoNgay(date, orders));
                }
                return new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Doanh thu",
                        Values = new ChartValues<float>(doanhThu)
                    }
                };
            }
            if (selectedChartData.Contains("THÁNG"))
            {
                // Get doanh thu of each day in the month
                List<float> doanhThu = new();
                for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
                {
                    DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i);
                    doanhThu.Add(BaoCaoViewModel.TinhDoanhThuTheoNgay(date, orders));
                }
                return new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Doanh thu",
                        Values = new ChartValues<float>(doanhThu)
                    }
                };
            }
            return new SeriesCollection();
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
