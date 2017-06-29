using System.Collections.Generic;
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
            ListView.ItemsSource = _listenerModelList;
        }
    }
}
