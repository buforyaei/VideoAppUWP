using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Windows.Foundation.Metadata;
using Windows.Networking.Connectivity;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TextUwpClient
{
    public sealed partial class MainPage : Page
    {
        private List<ListenerModel> _listenerModelList;
      
        public MainPage()
        {
           InitializeComponent();
           Loaded += OnLoaded;
        }
        private string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return null;
            var hostname =
                NetworkInformation.GetHostNames()
                    .SingleOrDefault(
                        hn =>
                            hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                            == icp.NetworkAdapter.NetworkAdapterId);

            // the ip address
            return hostname?.CanonicalName;
        }
        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var ip = GetLocalIp();
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
