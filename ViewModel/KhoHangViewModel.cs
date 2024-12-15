using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IT008_QuanLyBanHang.DTOs;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.ViewModel.API;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class KhoHangViewModel : ObservableObject, ITabViewModel
    {
        [ObservableProperty]
        private string searchText = string.Empty;

        private ObservableCollection<Product>? originalDataList;

        public KhoHangViewModel()
        {
            LoadDataCommand = new AsyncRelayCommand(LoadData);
            Task.Run(() => LoadData());

            this.PropertyChanged += OnViewModelPropertyChanged;

            this.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedProduct))
                {
                    await SelectProduct(SelectedProduct);
                } 
            };
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SearchText):
                    FilterData();
                    break;
                case nameof(SelectedTabIndex):
                    OnSelectedTabIndexChanged(SelectedTabIndex);
                    break;
            }
        }

        private void FilterData()
        {
            if (originalDataList == null)
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Products = new ObservableCollection<Product>(originalDataList);
            }
            else
            {
                // Tìm kiếm dưới dạng số nguyên nếu SearchText là số
                bool isNumeric = int.TryParse(SearchText, out int searchNumber);

                var filteredList = originalDataList.Where(item =>
                    item.ProductName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    item.Category != null && item.Category.CategoryName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    (isNumeric && item.Id == searchNumber)
                );
                Products = new ObservableCollection<Product>(filteredList);
            }
        }


        public IAsyncRelayCommand LoadDataCommand { get; }

        [RelayCommand]
        private void Expand(BatchProduct batchProduct)
        {
            if (batchProduct != null)
            {
                batchProduct.IsExpanded = !batchProduct.IsExpanded;
                Trace.WriteLine($"IsExpanded: {batchProduct.IsExpanded}");
            }
        }

        public async Task LoadData()
        {
            BatchProducts = await ProductAPI.GetAllProductsWithBatches();
            var p = await ProductAPI.GetAllProducts();
            if (p != null)
                Products = new ObservableCollection<Product>(p);


            // Chỉnh thứ tự lúc đầu theo tăng dần
            originalDataList = new ObservableCollection<Product>(
                Products?.OrderBy(p => p.Id)
            );

            Products = new ObservableCollection<Product>(originalDataList);
        }

        [RelayCommand]
        private void Hide()
        {
            if (SelectedItem == null)
                Trace.WriteLine("wtf null???");
            else
                SelectedItem.Visibility = Visibility.Collapsed;
        }

        [RelayCommand]
        private void AddProduct()
        {
            var taoHangHoaView = new IT008_QuanLyBanHang.View.TaoHangHoa();
            taoHangHoaView.Show();
        }

        [RelayCommand]
        private void AddBatch()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để nhập hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var nhapHangHoaView = new IT008_QuanLyBanHang.View.NhapHangHoaView();
            nhapHangHoaView.Show();

            WeakReferenceMessenger.Default.Send(new ProductSelectedMessage(SelectedProduct));
        }

        [ObservableProperty]
        ObservableCollection<Product>? products;

        [ObservableProperty]
        List<BatchProduct> batchProducts = new();

        [ObservableProperty]
        BatchProduct? selectedItem = null;

        [ObservableProperty]
        Product? selectedProduct;

        [ObservableProperty]
        ObservableCollection<Batch>? productBatches;

        [ObservableProperty]
        private int selectedTabIndex;

        [RelayCommand]
        private async Task SelectProduct(Product? product)
        {
            if (product == null)
                return;
            SelectedProduct = product;
            ProductBatches = new ObservableCollection<Batch>(await ProductAPI.GetBatchesOfProduct(SelectedProduct));
            Trace.WriteLine($"Selected Product: {SelectedProduct?.ProductName}, Batches Count: {ProductBatches?.Count}");
        }

        partial void OnSelectedTabIndexChanged(int value)
        {
            if (originalDataList == null) return;

            switch (value)
            {
                case 0: // "Tất cả" tab
                    Products = new ObservableCollection<Product>(originalDataList);
                    break;
                case 1: // "Đang trưng bày" tab
                    Products = new ObservableCollection<Product>(
                        originalDataList.Where(p => p.Status == "active")
                    );
                    break;
                default:
                    Products = new ObservableCollection<Product>(originalDataList);
                    break;
            }
        }

        [RelayCommand]
        private async Task Import()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            List<ProductDTO> importDatatList = new List<ProductDTO>();
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel Files|*.xlsx;*.xls",
                    Title = "Chọn file Excel"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    var package = new ExcelPackage(new FileInfo(openFileDialog.FileName));

                    // lấy ra sheet đầu tiên 
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (workSheet != null)
                    {
                        // duyệt tuần tự từ dòng thứ 2 đến dòng cuối cùng của file
                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            try
                            {
                                int j = 1;
                                int id;
                                int.TryParse(workSheet.Cells[i, j++].Value?.ToString(), out id);
                                string name = workSheet.Cells[i, j++].Value?.ToString() ?? "";
                                string image_url = workSheet.Cells[i, j++].Value?.ToString() ?? "";
                                string unit = workSheet.Cells[i, j++].Value?.ToString() ?? "";
                                int category_id = 0;
                                int.TryParse(workSheet.Cells[i, j++].Value?.ToString(), out category_id);

                                ProductDTO product = new ProductDTO()
                                {
                                    id = id,
                                    product_name = name,
                                    image_url = image_url,
                                    unit = unit,
                                    category_id = category_id
                                };

                                importDatatList.Add(product);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Lỗi khi đọc dòng {i}: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy worksheet trong file Excel!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
                return;
            }

            // Thêm dữ liệu vào DataList
            foreach (var data in importDatatList)
            {
                if (string.IsNullOrWhiteSpace(data.product_name) || data.category_id < 0|| string.IsNullOrWhiteSpace(data.unit))
                {
                    MessageBox.Show($"Thông tin của sản phẩm {data.id} {data.product_name} không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var productData = new Dictionary<string, string>
                {
                    { "product_name", data.product_name },
                    { "category_id", data.category_id.ToString()},
                    { "status", "active" },
                    { "unit", data.unit }
                };

                try
                {
                    var response = await RESTService.Instance.PostAsync("products", productData);
                    if (!string.IsNullOrEmpty(response))
                    {
                        MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        Trace.WriteLine(response);
                    }
                    else
                    {
                        MessageBox.Show("Thêm sản phẩm thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm sản phẩm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MessageBox.Show("Import dữ liệu thành công!");
        }

        [RelayCommand]
        private void Export()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            string filePath = "";
            // tạo SaveFileDialog để lưu file excel
            SaveFileDialog dialog = new SaveFileDialog();

            // chỉ lọc ra các file có định dạng Excel
            dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";

            // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
            }

            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Đường dẫn báo cáo không hợp lệ");
                return;
            }

            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    // đặt tên người tạo file
                    p.Workbook.Properties.Author = "AccountName";

                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "Báo cáo thống kê";

                    //Tạo một sheet để làm việc trên đó
                    var ws = p.Workbook.Worksheets.Add("New sheet");

                    // fontsize mặc định cho cả sheet
                    ws.Cells.Style.Font.Size = 11;
                    // font family mặc định cho cả sheet
                    ws.Cells.Style.Font.Name = "Calibri";

                    // Tạo danh sách các column header
                    string[] arrColumnHeader = {
                                                "Mã Hàng Hóa",
                                                "Tên Hàng Hóa",
                                                "Số Lượng",
                                                "Đơn vị",
                                                "Loại Hàng Hóa"
                };

                    // lấy ra số lượng cột cần dùng dựa vào số lượng header
                    var countColHeader = arrColumnHeader.Count();

                    // merge các column lại từ column 1 đến số column header
                    // gán giá trị cho cell vừa merge là Thống kê thông tni User Kteam
                    ws.Cells[1, 1].Value = "Thống kê hàng hóa";
                    ws.Cells[1, 1, 1, countColHeader].Merge = true;
                    // in đậm
                    ws.Cells[1, 1, 1, countColHeader].Style.Font.Bold = true;
                    // căn giữa
                    ws.Cells[1, 1, 1, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int colIndex = 1;
                    int rowIndex = 2;

                    //tạo các header từ column header đã tạo từ bên trên
                    foreach (var item in arrColumnHeader)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];

                        //set màu thành gray
                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        //căn chỉnh các border
                        var border = cell.Style.Border;
                        border.Bottom.Style =
                            border.Top.Style =
                            border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                        //gán giá trị
                        cell.Value = item;

                        colIndex++;
                    }

                    // Lấy ra danh sách hàng hóa từ DataList
                    var exportDataList = Products;

                    // Với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var item in exportDataList)
                    {
                        // Bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                        colIndex = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex++;

                        // Gán giá trị cho từng cell                      
                        ws.Cells[rowIndex, colIndex++].Value = item.Id;
                        ws.Cells[rowIndex, colIndex++].Value = item.ProductName;
                        ws.Cells[rowIndex, colIndex++].Value = item.Quantity;
                        ws.Cells[rowIndex, colIndex++].Value = item.Unit;
                        ws.Cells[rowIndex, colIndex++].Value = item.Category.CategoryName;
                    }

                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
                MessageBox.Show("Xuất excel thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi khi lưu file: {ex.Message}");
            }
        }
    }
}
