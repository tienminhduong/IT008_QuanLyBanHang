using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isLogin = true;

        [RelayCommand]
        void Login()
        {
            MainWindow mainWindow = new();
            IsLogin = false;
            mainWindow.ShowDialog();
            IsLogin = true;
        }

        [RelayCommand]
        void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
