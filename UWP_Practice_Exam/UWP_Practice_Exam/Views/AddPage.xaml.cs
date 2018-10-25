using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Practice_Exam.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddPage : Page
    {
        public AddPage()
        {
            this.InitializeComponent();
            
        }

        private async void Save_File(object sender, RoutedEventArgs e)
        {
            string name = myFileName.Text;
            string content = myContent.Text;
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await storageFolder.TryGetItemAsync(name);
            if (file == null)
            {
                file = await storageFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            }
            await Windows.Storage.FileIO.AppendTextAsync((StorageFile) file, content);
        }
    }
}
