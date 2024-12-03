using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.ViewModel.API;
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
        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(Login);
        }

        public IAsyncRelayCommand LoginCommand { get; }

        async Task Login()
        {
            Trace.WriteLine(Account);
            if (ShowLoginError = !(await Verify()))
                return;

            IsLogin = false;
            MainWindow mainWindow = new();
            ((MainWindowViewModel)mainWindow.DataContext).SwitchToViewCommand.Execute("Button_DonHang");
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
