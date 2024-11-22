﻿using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Windows.Controls;
using System.Windows.Media;

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

            currentView = viewDictionary["Button_DonHang"];
            currentViewName = "Button_DonHang";
        }

        [RelayCommand]
        void SwitchToView(System.Windows.Controls.Button button)
        {
            CurrentView = viewDictionary[button.Name];
            CurrentViewName = button.Name;
        }

        [ObservableProperty]
        private ObservableObject currentView;
        [ObservableProperty]
        private string currentViewName;

        private readonly Dictionary<string, MainWindowTabViewModel> viewDictionary;

        //private Color ActiveTabColor = Color.FromArgb(75, 87, 104);
        //private Color InactiveTabColor = Color.FromArgb(30, 42, 57);


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

    // converter
    public class IsActiveToColorConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string currentViewName && parameter is string tabButtonName)
            {
                Trace.WriteLine($"Current view name: {currentViewName}, tabButtonName: {tabButtonName}");
                if (currentViewName == tabButtonName)
                {
                    Trace.WriteLine("It's f***ing correct");
                    return new SolidColorBrush(activeColor);
                }
            }
            Trace.WriteLine("It's f***ing incorrect");
            return new SolidColorBrush(inactiveColor);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        readonly private System.Windows.Media.Color activeColor = System.Windows.Media.Color.FromRgb(75, 87, 104);
        readonly private System.Windows.Media.Color inactiveColor = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
    }
}
