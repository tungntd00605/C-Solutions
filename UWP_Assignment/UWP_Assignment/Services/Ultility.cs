using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWP_Assignment.Services
{
    class Ultility
    {
        public static void HideActiveContentDialog()
        {
            var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
            foreach (var popup in popups)
            {
                if (popup.Child is ContentDialog)
                {
                    ContentDialog currentDialog = (ContentDialog)popup.Child;
                    currentDialog.Hide();
                }
            }
        }
    }
}
