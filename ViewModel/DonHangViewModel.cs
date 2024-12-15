using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class DonHangViewModel : ObservableObject, ITabViewModel
    {
        public DonHangViewModel()
        {
            Task.Run(() => LoadData());
        }

        public async Task LoadData()
        {
            orders = await OrderAPI.GetAllOrders();
            OrderShown = new(orders);
        }

        [ObservableProperty] ObservableCollection<Order> orderShown = new();
        List<Order> orders = new();

        [ObservableProperty] DataGridRowDetailsVisibilityMode showDetails = DataGridRowDetailsVisibilityMode.Collapsed;

        [ObservableProperty] string searchText = "";

        [RelayCommand]
        void TaoDonHang()
        {
            var taoDonHangWindow = new IT008_QuanLyBanHang.View.TaoDonHangView();
            taoDonHangWindow.ShowDialog();
            Task.Run(() => LoadData());
        }

        [RelayCommand]
        void Search()
        {
            if (string.IsNullOrEmpty(SearchText))
                OrderShown = new(orders);
            else
                OrderShown = new(orders.Where(o => FunctionTool.CheckContains(o.Customer?.FullName??"", SearchText)));
        }

        [ObservableProperty] DateTime searchDate = DateTime.Today;
        [ObservableProperty] string searchButtonText = "Tìm ngày";

        [RelayCommand]
        async Task ExportToExcel()
        {
            // Get the file path using SaveFileDialog
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".xlsx",
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };
            var result = dialog.ShowDialog();
            if (result != true)
                return;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                // Add headers
                worksheet.Cells[1, 1].Value = "Mã đơn hàng";
                worksheet.Cells[1, 2].Value = "Tên khách hàng";
                worksheet.Cells[1, 3].Value = "Tổng trị giá";
                worksheet.Cells[1, 4].Value = "Ngày mua hàng";

                // Add data
                for (int i = 0; i < OrderShown.Count; i++)
                {
                    var order = OrderShown[i];
                    worksheet.Cells[i + 2, 1].Value = order.Id;
                    worksheet.Cells[i + 2, 2].Value = order.Customer?.FullName;
                    worksheet.Cells[i + 2, 3].Value = order.TotalAmount;
                    worksheet.Cells[i + 2, 4].Value = order.OrderDate.ToString("yyyy-MM-dd");
                }

                // Save the file
                FileInfo fileInfo = new FileInfo(dialog.FileName);
                await package.SaveAsAsync(fileInfo);
            }

            // Notify the user
            System.Windows.MessageBox.Show($"Lưu thành công", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        [RelayCommand]
        async Task ExportAllToExcel()
        {
            // Get the file path using SaveFileDialog
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".xlsx",
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };
            var result = dialog.ShowDialog();
            if (result != true)
                return;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");
                // Add headers
                worksheet.Cells[1, 1].Value = "Mã đơn hàng";
                worksheet.Cells[1, 2].Value = "Tên khách hàng";
                worksheet.Cells[1, 3].Value = "Tổng trị giá";
                worksheet.Cells[1, 4].Value = "Ngày mua hàng";
                // Add data
                for (int i = 0; i < orders.Count; i++)
                {
                    var order = orders[i];
                    worksheet.Cells[i + 2, 1].Value = order.Id;
                    worksheet.Cells[i + 2, 2].Value = order.Customer?.FullName;
                    worksheet.Cells[i + 2, 3].Value = order.TotalAmount;
                    worksheet.Cells[i + 2, 4].Value = order.OrderDate.ToString("yyyy-MM-dd");
                }
                // Save the file
                FileInfo fileInfo = new FileInfo(dialog.FileName);
                await package.SaveAsAsync(fileInfo);
            }
            // Notify the user
            System.Windows.MessageBox.Show($"Lưu thành công", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

            [RelayCommand]
        void SearchByDate()
        {
            if (SearchButtonText == "Hủy tìm")
            {
                OrderShown = new(orders);
                SearchButtonText = "Tìm ngày";
                return;
            }

            OrderShown = new(orders.Where(o => o.OrderDate.Date == SearchDate.Date));
            SearchButtonText = "Hủy tìm";
        }
    }
}
