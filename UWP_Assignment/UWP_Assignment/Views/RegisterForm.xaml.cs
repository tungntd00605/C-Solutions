using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Assignment.Entity;
using UWP_Assignment.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterForm : Page
    {
        CheckValid checkRegister = new CheckValid();
        private static StorageFile file;
        private static string UploadUrl;

        public RegisterForm()
        {
            this.InitializeComponent();
            GetUploadUrl();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        // Do Signup and auto login
        private async void Handle_Signup(object sender, RoutedEventArgs e)
        {
            // Check All Validate
            checkRegister.IsAllValid = checkRegister.CheckEmail(Email, Email_Message, Email_Symbol) & checkRegister.CheckPassword(Password, Password_Message, Password_Symbol)
                & checkRegister.CheckFirstName(FirstName, FirstName_Message, FirstName_Symbol) & checkRegister.CheckLastName(LastName, LastName_Message, LastName_Symbol)
                & checkRegister.CheckPhone(Phone, Phone_Message, Phone_Symbol) & checkRegister.CheckAddress(Address, Address_Message, Address_Symbol);

            if (checkRegister.IsAllValid == true)
            {
                App.currentMember.firstName = this.FirstName.Text;
                App.currentMember.lastName = this.LastName.Text;
                App.currentMember.email = this.Email.Text;
                App.currentMember.password = this.Password.Password.ToString();
                if(string.IsNullOrEmpty(this.ImageUrl.Text))
                {
                    App.currentMember.avatar = "https://melbournechapter.net/images/log-clipart-avatar.png";
                }
                else
                {
                    App.currentMember.avatar = this.ImageUrl.Text;
                }
                App.currentMember.phone = this.Phone.Text;
                App.currentMember.address = this.Address.Text;
                App.currentMember.introduction = this.Introduction.Text;
                // Handle post to api here (Do later)
                await API_Handle.Sign_Up(App.currentMember);

                // Need to handle error if sign up fail (Do later)
                //Hanle login after sign up success
                Dictionary<String, String> LoginInfor = new Dictionary<string, string>();
                LoginInfor.Add("email", App.currentMember.email);
                LoginInfor.Add("password", App.currentMember.password);
                await API_Handle.Login(LoginInfor);
            }
        }

        // Capture photo by camera and upload to server
        private async void Capture_Photo(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
            file = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file == null)
            {
                // User cancelled photo capture
                return;
            }
            HttpUploadFile(UploadUrl, "myFile", "image/png");
        }

        private static async void GetUploadUrl()
        {
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
            Uri requestUri = new Uri("https://1-dot-backup-server-002.appspot.com/get-upload-token");
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";
            try
            {
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            UploadUrl = httpResponseBody;
        }

        // Upload image to server
        public async void HttpUploadFile(string url, string paramName, string contentType)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";

            Stream rs = await wr.GetRequestStreamAsync();
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, "path_file", contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            // write file.
            Stream fileStream = await file.OpenStreamForReadAsync();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);

            WebResponse wresp = null;
            try
            {
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //Debug.WriteLine(string.Format("File uploaded, server response is: @{0}@", reader2.ReadToEnd()));
                //string imgUrl = reader2.ReadToEnd();
                Uri u = new Uri(reader2.ReadToEnd(), UriKind.Absolute);
                Debug.WriteLine(u.AbsoluteUri);
                ImageUrl.Text = u.AbsoluteUri;
                MyAvatar.Source = new BitmapImage(u);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error uploading file", ex.StackTrace);
                Debug.WriteLine("Error uploading file", ex.InnerException);
                if (wresp != null)
                {
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        //Go back button (Simple)
        private void Go_Back(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        // Choose Gender and add to member object
        private void Select_Gender(object sender, RoutedEventArgs e)
        {
            RadioButton radioGender = sender as RadioButton;
            App.currentMember.gender = Int32.Parse(radioGender.Tag.ToString());
        }

        // Choose birthday and add to member object
        private void Change_Birthday(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            // Validate birthday here (Do later)
            App.currentMember.birthday = sender.Date.Value.ToString("yyyy-MM-dd");
        }

        // Reset register form button (Do later)
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // Do validate and post signup information

        // Navigate to Login form
        private void To_Login(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            this.Frame.Navigate(typeof(Views.LoginForm));
        }

        // Validate handle events
        private void Email_GotFocus(object sender, RoutedEventArgs e)
        {
            Email_Symbol.Visibility = Visibility.Collapsed;
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            Password_Symbol.Visibility = Visibility.Collapsed;
        }

        private void FirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            FirstName_Symbol.Visibility = Visibility.Collapsed;
        }

        private void LastName_GotFocus(object sender, RoutedEventArgs e)
        {
            LastName_Symbol.Visibility = Visibility.Collapsed;
        }

        private void Phone_GotFocus(object sender, RoutedEventArgs e)
        {
            Phone_Symbol.Visibility = Visibility.Collapsed;
        }

        private void Address_GotFocus(object sender, RoutedEventArgs e)
        {
            Address_Symbol.Visibility = Visibility.Collapsed;
        }

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            checkRegister.CheckEmail(Email, Email_Message, Email_Symbol);
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            checkRegister.CheckPassword(Password, Password_Message, Password_Symbol);
        }

        private void FirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkRegister.CheckFirstName(FirstName, FirstName_Message, FirstName_Symbol);
        }

        private void LastName_LostFocus(object sender, RoutedEventArgs e)
        {
            checkRegister.CheckLastName(LastName, LastName_Message, LastName_Symbol);
        }

        private void Phone_LostFocus(object sender, RoutedEventArgs e)
        {
            checkRegister.CheckPhone(Phone, Phone_Message, Phone_Symbol);
        }

        private void Address_LostFocus(object sender, RoutedEventArgs e)
        {
            checkRegister.CheckAddress(Address, Address_Message, Address_Symbol);
        }

        
    }
}
