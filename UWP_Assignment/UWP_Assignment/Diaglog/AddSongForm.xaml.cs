using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Assignment.Entity;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Diaglog
{
    public sealed partial class AddSongForm : ContentDialog
    {
        public AddSongForm()
        {
            this.InitializeComponent();
        }

        private async void Handle_Add_Song(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Validate comes here (Do later)
            Song newSong = new Song();
            newSong.name = SongName.Text;
            newSong.singer = Singer.Text;
            newSong.author = Author.Text;
            if (string.IsNullOrEmpty(Thumbnail.Text))
            {
                newSong.thumbnail = "https://mbtskoudsalg.com/images/song-note-png-8.png";
            }
            else
            {
                newSong.thumbnail = Thumbnail.Text;
            }
            newSong.link = Link.Text;
            newSong.description = Description.Text;
            await API_Handle.Create_Song(newSong);
        }
    }
}
