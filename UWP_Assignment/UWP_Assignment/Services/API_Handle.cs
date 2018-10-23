using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UWP_Assignment.Entity;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWP_Assignment.Services
{
    class API_Handle
    {
        private static string API_REGISTER = "http://1-dot-backup-server-002.appspot.com/_api/v2/members";
        private static string API_LOGIN = "http://2-dot-backup-server-002.appspot.com/_api/v2/members/authentication";
        private static string SONG_API_URL = "https://2-dot-backup-server-002.appspot.com/_api/v2/songs";
        private static string API_INFO = "http://2-dot-backup-server-002.appspot.com/_api/v2/members/information";

        public static async Task<string> Sign_Up(Member member)
        {
            HttpClient httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(member), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_REGISTER, content);
            var contents = await response.Result.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);
            return contents;
        }

        public static async Task<HttpResponseMessage> Get_Information()
        {
            // Lay thong tin ca nhan bang token.
            HttpClient client2 = new HttpClient();
            client2.DefaultRequestHeaders.Add("Authorization", "Basic " + App.token.token);
            var resp = client2.GetAsync(API_INFO).Result;
            
            return resp;
        }
        
        public static async Task<string> Login(Dictionary<string, string> LoginInfor)
        {
            HttpClient httpClient = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(LoginInfor), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(API_LOGIN, content).Result;
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                App.isLogin = true;
                // save file...
                Debug.WriteLine(responseContent);
                // Doc token
                App.token = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                // Luu token
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.CreateFileAsync("token.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, responseContent);
            }
            else
            {
                // Xu ly loi.
                ErrorResponse errorObject = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                Debug.WriteLine("Message: " + errorObject.message);
                
                if (errorObject != null && errorObject.error.Count > 0)
                {
                    var textMessage = errorObject.message;
                    StringBuilder textErrors = new StringBuilder();

                    foreach (var key in errorObject.error.Keys)
                    {
                        string textError = errorObject.error[key] + "\n";
                        textErrors.Append(textError);
                    }

                    ContentDialog errorDiaglog = new ContentDialog
                    {
                        Title = textMessage,
                        Content = textErrors,
                        CloseButtonText = "Ok"
                    };

                    ContentDialogResult result = await errorDiaglog.ShowAsync();
                }
            }
            return responseContent;
        }

        public async static Task<string> Create_Song(Song song)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + App.token.token);
            var content = new StringContent(JsonConvert.SerializeObject(song), System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(SONG_API_URL, content).Result;
            
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Ultility.HideActiveContentDialog();
                // Sample success message
                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Success",
                    Content = "Your song has been added",
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await successDialog.ShowAsync();
            }
            else
            {
                Ultility.HideActiveContentDialog();
                // Sample error message
                ContentDialog errorDiaglog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Nothing added! Please try again later",
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await errorDiaglog.ShowAsync();
            }
            var contents = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);
            return contents;
        }

        public static HttpResponseMessage Get_Lastest_Songs() { 
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + App.token.token);
            var response = httpClient.GetAsync(SONG_API_URL).Result;
            //var json = response.Content.ReadAsStringAsync().ToString();
            //var array = JsonConverter.
            //Debug.WriteLine(JsonConvert.DeserializeObject<Song>(json));
            return response;
        }
    }
}
