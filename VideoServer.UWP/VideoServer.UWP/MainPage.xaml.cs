using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TextUwpClient;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VideoServer.UWP
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
            Utlis.ShowStatusBar();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            InitializeWebService();
        }

        private async void InitializeWebService()
        {
            var ip = Utlis.GetLocalIp();
            if (!await WebServiceClient.Instance.PostIpData(ip)) return;
            _listenerModelList = new List<ListenerModel>
            {
                new ListenerModel("40403"),
                new ListenerModel("40404"),
                new ListenerModel("40405"),
                new ListenerModel("40406"),
                new ListenerModel("40407"),
                new ListenerModel("40408"),
            };
            foreach (var listener in _listenerModelList)
            {
                listener.OnConnectedCamera += OnConnectedCamera;
                listener.OnError += OnError;
            }
            ListView.ItemsSource = _listenerModelList;
        }

        private void OnError(object sender, string s)
        {
            try
            {
                var newList = _listenerModelList.Where(c => c.Port != s);
                ListView.ItemsSource = newList;
            }
            catch
            {
                
            }
            
        }

        private async void OnConnectedCamera(object sender, EventArgs eventArgs)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ProgressRing.Visibility = Visibility.Collapsed;
                WaitingTextBox.Visibility = Visibility.Collapsed;
            });
        }

    }
}
