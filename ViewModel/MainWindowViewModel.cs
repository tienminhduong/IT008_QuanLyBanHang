using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            viewDictionary ??= new();
            viewDictionary.Add("Button_TongQuan", new TongQuanViewModel());
            viewDictionary.Add("Button_DonHang", new DonHangViewModel());
            viewDictionary.Add("Button_TaoDonHang", new TaoDonHangViewModel());
            viewDictionary.Add("Button_KhoHang", new KhoHangViewModel());
            viewDictionary.Add("Button_KhachHang", new KhachHangViewModel());
            viewDictionary.Add("Button_BaoCao", new BaoCaoViewModel());


            currentView = viewDictionary["Button_TongQuan"];

            LoadDataCommand.Execute(null);
        }

        [RelayCommand]
        void SwitchToView(System.Windows.Controls.Button button)
        {
            CurrentView = viewDictionary[button.Name];
        }

        [RelayCommand]
        private async Task LoadData()
        {
            string temp = await REST_Service.Instance.GetAsync("products");
            Application.Current.Dispatcher.Invoke(() => MessageBox.Show(temp));
        }



        [ObservableProperty]
        private ObservableObject currentView;

        private Dictionary<string, ObservableObject> viewDictionary;
    }
}
