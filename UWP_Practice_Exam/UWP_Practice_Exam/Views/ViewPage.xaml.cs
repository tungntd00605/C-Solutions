using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ViewPage : Page
    {
        ObservableCollection<File> myListFile = new ObservableCollection<File>();
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
                File newFile = new File();
                newFile.name = file.Name;
                newFile.content = await Windows.Storage.FileIO.ReadTextAsync(file);
                myListFile.Add(newFile);
            }
        }

        private void myComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
