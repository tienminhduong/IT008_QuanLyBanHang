using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IT008_QuanLyBanHang.Interfaces;
using IT008_QuanLyBanHang.ViewModel.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IT008_QuanLyBanHang.ViewModel
{
    public partial class ThietLapViewModel : ObservableObject, ITabViewModel
    {
        public ThietLapViewModel()
        {
            
        }

        public async Task LoadData()
        {
            await Task.Delay(0);
        }

        [RelayCommand]
        async Task ChangePassword()
        {
            if (NewPassword != ConfirmPassword)
            {
                // show error message
                MessageBox.Show("Mật khẩu mới không khớp", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // call API to change password
            string body = $"{{\"current_password\": \"{Password}\", \"new_password\": \"{NewPassword}\", \"new_password_confirmation\": \"{ConfirmPassword}\"}}";
            try
            {
                await RESTService.Instance.PutAsync("users/current/password", body);
                MessageBox.Show("Đổi mật khẩu thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Sai mật khẩu", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

            [ObservableProperty]
        string password = "";
        [ObservableProperty]
        string newPassword = "";
        [ObservableProperty]
        string confirmPassword = "";
    }
}
