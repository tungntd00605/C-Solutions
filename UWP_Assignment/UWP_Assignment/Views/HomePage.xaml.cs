using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Song> listSong;
        
        internal ObservableCollection<Song> ListSong { get => listSong; set => listSong = value; }
        internal string UserName = App.currentMember.lastName + " " + App.currentMember.firstName;
        MediaPlayer _mediaPlayer;
        MediaSource _mediaSource;
        MediaPlaybackItem _mediaPlaybackItem;
        MediaPlaybackList _mediaPlaybackList;
        MediaPlaybackSession _playBackSession;

        public HomePage()
        {
            this.InitializeComponent();

            //API_Handle.Get_Lastest_Songs();
            this.ListSong = new ObservableCollection<Song>();
            this.ListSong.Add(new Song()
            {
                name = "Chưa bao giờ",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://file.tinnhac.com/resize/600x-/music/2017/07/04/19554480101556946929-b89c.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/ChuaBaoGioSEESINGSHARE2-HaAnhTuan-5111026.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Tình thôi xót xa",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://i.ytimg.com/vi/XyjhXzsVdiI/maxresdefault.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/TinhThoiXotXaSEESINGSHARE1-HaAnhTuan-4652191.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Tháng tư là tháng nói dối của em",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://sky.vn/wp-content/uploads/2018/05/0-30.jpg",
                link = "https://od.lk/s/NjFfMjM4MzQ1OThf/ThangTuLaLoiNoiDoiCuaEm-HaAnhTuan-4609544.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Nơi ấy bình yên",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://i.ytimg.com/vi/A8u_fOetSQc/hqdefault.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui946/NoiAyBinhYenSeeSingShare2-HaAnhTuan-5085337.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Giấc mơ chỉ là giấc mơ",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://i.ytimg.com/vi/J_VuNwxSEi0/maxresdefault.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui945/GiacMoChiLaGiacMoSeeSingShare2-HaAnhTuan-5082049.mp3"
            });
            this.ListSong.Add(new Song()
            {
                name = "Người tình mùa đông",
                singer = "Hà Anh Tuấn",
                thumbnail = "https://i.ytimg.com/vi/EXAmxBxpZEM/maxresdefault.jpg",
                link = "https://c1-ex-swe.nixcdn.com/NhacCuaTui963/NguoiTinhMuaDongSEESINGSHARE2-HaAnhTuan-5104816.mp3"
            });

            _mediaPlaybackList = new MediaPlaybackList();
            foreach (var Song in ListSong)
            {
                _mediaSource = MediaSource.CreateFromUri(new Uri(Song.link));
                _mediaPlaybackItem = new MediaPlaybackItem(_mediaSource);
                _mediaPlaybackList.Items.Add(_mediaPlaybackItem);
            }
            _mediaPlaybackList.CurrentItemChanged += _mediaPlaybackList_CurrentItemChanged;
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.AutoPlay = false;
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
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.MenuList.SelectedIndex = (int)_mediaPlaybackList.CurrentItemIndex;
            });
        }

        private void MenuList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine(MenuList.Items.IndexOf(e.ClickedItem));
            _mediaPlaybackList.MoveTo((uint)MenuList.Items.IndexOf(e.ClickedItem));
            if(_playBackSession.PlaybackState != MediaPlaybackState.Playing)
            {
                _mediaPlayer.Play();
            }
            playButton.Icon = new SymbolIcon(Symbol.Pause);
        }
    }
}
