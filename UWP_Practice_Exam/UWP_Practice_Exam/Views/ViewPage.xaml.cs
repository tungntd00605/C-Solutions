using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class ViewPage : Page
    {
        ObservableCollection<string> myListFile = new ObservableCollection<string>();
        IReadOnlyList<StorageFile> listFile;
        public ViewPage()
        {
            this.InitializeComponent();
            Get_All_File();
        }

        public async void Get_All_File()
        {
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            listFile = await storageFolder.GetFilesAsync();
            foreach (StorageFile file in listFile)
            {
                myListFile.Add(file.Name);
            }
        }

        public async Task<string> Get_File_Content(string fileName)
        {
            StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var currentFile  = await storageFolder.GetFileAsync(fileName);
            string content = await Windows.Storage.FileIO.ReadTextAsync(currentFile);
            return content;
        }

        private async void myComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            myContent.Text = await Get_File_Content(box.SelectedValue.ToString());
        }
    }
}
