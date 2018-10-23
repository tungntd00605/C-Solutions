using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Assignment.Entity;
using UWP_Assignment.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_Assignment
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            AutoLogin();
            
        }

        private async void AutoLogin()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync("token.txt");
            string token = await Windows.Storage.FileIO.ReadTextAsync(file);
            App.token = JsonConvert.DeserializeObject<TokenResponse>(token);
            HttpResponseMessage response = await API_Handle.Get_Information();
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                MainFrame.Navigate(typeof(Views.LoginForm));
                // Notice user the token is now invalid (Do later)
            }
            else
            {
                App.isLogin = true;
                var contents = await response.Content.ReadAsStringAsync();
                App.currentMember = JsonConvert.DeserializeObject<Member>(contents);
                MainFrame.Navigate(typeof(Views.HomePage));
            }
        }
    }
}
