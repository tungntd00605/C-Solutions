using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Assignment.Diaglog;
using UWP_Assignment.Entity;
using UWP_Assignment.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
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
    public sealed partial class HomePage : Page
    {
        private int playingList = 0; //0 = main list; 1 = my list...
        private ObservableCollection<Song> listSong;
        private ObservableCollection<Song> myListSong;

        internal ObservableCollection<Song> ListSong { get => listSong; set => listSong = value; }
        internal ObservableCollection<Song> MyListSong { get => myListSong; set => myListSong = value; }
        internal string UserName = App.currentMember.firstName;
        MediaPlayer _mediaPlayer;
        MediaSource _mediaSource;
        MediaPlaybackItem _mediaPlaybackItem;
        MediaPlaybackList _mediaPlaybackList;
        MediaPlaybackSession _playBackSession;

        public HomePage()
        {
            this.InitializeComponent();
            this.ListSong = API_Handle.Get_Lastest_Songs().Result;
            this.MyListSong = API_Handle.Get_My_Songs().Result;

            _mediaPlaybackList = new MediaPlaybackList();
            foreach (var Song in ListSong)
            {
                _mediaSource = MediaSource.CreateFromUri(new Uri(Song.link));
                _mediaPlaybackItem = new MediaPlaybackItem(_mediaSource);
                var props = _mediaPlaybackItem.GetDisplayProperties();
                props.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(Song.thumbnail));
                props.Type = Windows.Media.MediaPlaybackType.Music;
                props.MusicProperties.Title = Song.name;
                props.MusicProperties.Artist = Song.singer;
                _mediaPlaybackItem.ApplyDisplayProperties(props);
                _mediaPlaybackList.Items.Add(_mediaPlaybackItem);
            }
            _mediaPlaybackList.CurrentItemChanged += _mediaPlaybackList_CurrentItemChanged;
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.AutoPlay = false;
            volumeSlider.Value = _mediaPlayer.Volume*100;
            _mediaPlayer.Source = _mediaPlaybackList;
            myMediaPlayer.SetMediaPlayer(_mediaPlayer);
            _playBackSession = _mediaPlayer.PlaybackSession;
        }

        private async void Handle_Logout(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Warning!",
                Content = "Do you want to logout ?",
                PrimaryButtonText = "Yes",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await dialog.ShowAsync();
            if(result == ContentDialogResult.Primary)
            {
                App.isLogin = false;
                App.currentMember = new Member();
                Ultility.HideActiveContentDialog();
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.GetFileAsync("token.txt");
                await file.DeleteAsync();
                this.Frame.Navigate(typeof(Views.LoginForm));
            }            
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AddSongForm songForm = new AddSongForm();
            await songForm.ShowAsync();
        }

        private void Play_Button(object sender, RoutedEventArgs e)
        {
            if (_playBackSession.PlaybackState != MediaPlaybackState.Playing)
            {
                _mediaPlayer.Play();
                playButton.Icon = new SymbolIcon(Symbol.Pause);
            } else
            {
                _mediaPlayer.Pause();
                playButton.Icon = new SymbolIcon(Symbol.Play);
            }
        }

        private void Next_Button(object sender, RoutedEventArgs e)
        {           
            _mediaPlaybackList.MoveNext();
        }
        
        private void Previous_Button(object sender, RoutedEventArgs e)
        {
            _mediaPlaybackList.MovePrevious();
        }

        private async void _mediaPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            if (playingList == 0)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.MenuList.SelectedIndex = (int)_mediaPlaybackList.CurrentItemIndex;
                });                
            }
            else if (playingList == 1)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    this.MySongList.SelectedIndex = (int)_mediaPlaybackList.CurrentItemIndex;
                });                
            }            
        }

        private void MenuList_ItemClick(object sender, ItemClickEventArgs e)
        {            
            if(playingList != 0)
            {
                _mediaPlaybackList = new MediaPlaybackList();
                foreach (var Song in ListSong)
                {
                    _mediaSource = MediaSource.CreateFromUri(new Uri(Song.link));
                    _mediaPlaybackItem = new MediaPlaybackItem(_mediaSource);
                    var props = _mediaPlaybackItem.GetDisplayProperties();
                    props.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(Song.thumbnail));
                    props.Type = Windows.Media.MediaPlaybackType.Music;
                    props.MusicProperties.Title = Song.name;
                    props.MusicProperties.Artist = Song.singer;
                    _mediaPlaybackItem.ApplyDisplayProperties(props);
                    _mediaPlaybackList.Items.Add(_mediaPlaybackItem);
                }
                _mediaPlayer.Source = _mediaPlayer.Source = _mediaPlaybackList;                
                playingList = 0;
            }
            _mediaPlaybackList.MoveTo((uint)MenuList.Items.IndexOf(e.ClickedItem));
            if(_playBackSession.PlaybackState != MediaPlaybackState.Playing)
            {
                _mediaPlayer.Play();
            }
            playButton.Icon = new SymbolIcon(Symbol.Pause);
        }

        private void MySongList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (playingList != 1)
            {
                _mediaPlaybackList = new MediaPlaybackList();
                foreach (var Song in MyListSong)
                {
                    _mediaSource = MediaSource.CreateFromUri(new Uri(Song.link));
                    _mediaPlaybackItem = new MediaPlaybackItem(_mediaSource);
                    var props = _mediaPlaybackItem.GetDisplayProperties();
                    props.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(Song.thumbnail));
                    props.Type = Windows.Media.MediaPlaybackType.Music;
                    props.MusicProperties.Title = Song.name;
                    props.MusicProperties.Artist = Song.singer;
                    _mediaPlaybackItem.ApplyDisplayProperties(props);
                    _mediaPlaybackList.Items.Add(_mediaPlaybackItem);
                }
                _mediaPlayer.Source = _mediaPlayer.Source = _mediaPlaybackList;                
                playingList = 1;
            }
            _mediaPlaybackList.MoveTo((uint)MySongList.Items.IndexOf(e.ClickedItem));
            if (_playBackSession.PlaybackState != MediaPlaybackState.Playing)
            {
                _mediaPlayer.Play();
            }
            playButton.Icon = new SymbolIcon(Symbol.Pause);
        }

        private void Shuffle_Button(object sender, RoutedEventArgs e)
        {
            _mediaPlaybackList.ShuffleEnabled = !_mediaPlaybackList.ShuffleEnabled;            
        }

        private void Auto_Repeat_Button(object sender, RoutedEventArgs e)
        {
            _mediaPlaybackList.AutoRepeatEnabled = !_mediaPlaybackList.AutoRepeatEnabled;
        }

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                _mediaPlayer.Volume = slider.Value/100;
            }
        }

        private async void Refresh_List_Button(object sender, RoutedEventArgs e)
        {
            var notifyDialog = new ContentDialog
            {
                Title = "In Development",
                Content = "Sorry this function is currently in development",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await notifyDialog.ShowAsync();
        }
        
    }
}
