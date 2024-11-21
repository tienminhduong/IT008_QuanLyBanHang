using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Model;
using IT008_QuanLyBanHang.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
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
            viewDictionary.Add("Button_KhoHang", new KhoHangViewModel());
            viewDictionary.Add("Button_KhachHang", new KhachHangViewModel());
            viewDictionary.Add("Button_BaoCao", new BaoCaoViewModel());

            tabCurrentColor ??= new();
            tabCurrentColor.Add("Button_TongQuan", InactiveTabColor);
            tabCurrentColor.Add("Button_DonHang", InactiveTabColor);
            tabCurrentColor.Add("Button_KhoHang", InactiveTabColor);
            tabCurrentColor.Add("Button_KhachHang", InactiveTabColor);
            tabCurrentColor.Add("Button_BaoCao", InactiveTabColor);

            currentView = viewDictionary["Button_DonHang"];
            tabCurrentColor["Button_DonHang"] = ActiveTabColor;
        }

        [RelayCommand]
        void SwitchToView(System.Windows.Controls.Button button)
        {
            CurrentView = viewDictionary[button.Name];
        }

        [ObservableProperty]
        private ObservableObject currentView;

        private Dictionary<string, MainWindowTabViewModel> viewDictionary;
        [ObservableProperty]
        private Dictionary<string, Color> tabCurrentColor;

        private Color ActiveTabColor = Color.FromArgb(75, 87, 104);
        private Color InactiveTabColor = Color.FromArgb(30, 42, 57);


        public bool IsLoadedComplete
        {
            get
            {
                foreach (var view in viewDictionary.Values)
                {
                    if (!view.IsLoadedComplete)
                        return false;
                }
                return true;
            }
        }

    }
}
