using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        [RelayCommand]
        async Task Login()
        {
            Trace.WriteLine(Account);
            if (ShowLoginError = !(await Verify()))
                return;

            MainWindow mainWindow = new();
            while (!((MainWindowViewModel)mainWindow.DataContext).IsLoadedComplete) ;
            IsLogin = false;
            mainWindow.ShowDialog();
            IsLogin = true;
        }

        [RelayCommand]
        void Exit()
        {
            Application.Current.Shutdown();
        }

        async Task<bool> Verify()
        {
            if (RESTService.Instance == null)
                return false;

            return (await RESTService.Instance.TryLogin(Account, Password));
        }

        [ObservableProperty]
        bool isLogin = true;

        [ObservableProperty]
        string account = "";

        [ObservableProperty]
        string password = "";

        [ObservableProperty]
        bool showLoginError = false;

    }

}
