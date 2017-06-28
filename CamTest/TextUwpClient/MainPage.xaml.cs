using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Media.MediaProperties;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TextUwpClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<ListenerModel> _listenerModelList;
      
        public MainPage()
        {
           InitializeComponent();
           Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _listenerModelList = new List<ListenerModel>
            {
                new ListenerModel("40404"),
                new ListenerModel("40405"),
                new ListenerModel("40406"),
                new ListenerModel("40407"),
                new ListenerModel("40408"),
                new ListenerModel("40409"),
            };
            ListView.ItemsSource = _listenerModelList;
            if (!ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar")) return;
            var statusbar = StatusBar.GetForCurrentView();
            statusbar.BackgroundColor = Colors.Black;
            statusbar.BackgroundOpacity = 1;
            statusbar.ForegroundColor = Colors.White;
            await statusbar.ShowAsync();

        }


    }
}
