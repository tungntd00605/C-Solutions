using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Assignment.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginForm : Page
    {
        CheckValid checkLogin = new CheckValid();

        public LoginForm()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private async void Handle_Login(object sender, RoutedEventArgs e)
        {
            // Validate

            checkLogin.IsAllValid = checkLogin.CheckPassword(Password, Password_Message, Password_Symbol)
            & checkLogin.CheckEmail(Email, Email_Message, Email_Symbol);

            // Process post login data and get token (Do later)
            if (checkLogin.IsAllValid == true)
            {
                Dictionary<String, String> LoginInfor = new Dictionary<string, string>();
                LoginInfor.Add("email", Email.Text);
                LoginInfor.Add("password", Password.Password);
                await API_Handle.Login(LoginInfor);
            }
            else
            {
                // Thong bao loi~ (Do later)
            }
        }

        private void To_Register(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            this.Frame.Navigate(typeof(Views.RegisterForm));
        }

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            checkLogin.CheckEmail(Email, Email_Message, Email_Symbol);
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            checkLogin.CheckPassword(Password, Password_Message, Password_Symbol);
        }

        private void Email_GotFocus(object sender, RoutedEventArgs e)
        {
            Email_Symbol.Visibility = Visibility.Collapsed;
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            Password_Symbol.Visibility = Visibility.Collapsed;
        }
    }
}
