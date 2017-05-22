using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VideoStreamApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private VideoProvider _videoProvider;
        private VideoReciever _videoReciever1;
        private VideoReciever _videoReciever2;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ButtonConnect_OnClick(object sender, RoutedEventArgs e)
        {
            _videoReciever1 = new VideoReciever(Image1, "40404");
            await _videoReciever1.Initialize(Host.Text);
            _videoReciever1.StartListening();
        }
        private async void ButtonHost_OnClick(object sender, RoutedEventArgs e)
        {
            _videoProvider = new VideoProvider(new TextBlock(), Port.Text);
            await _videoProvider.InitializeProvider();
        }
        private async void ButtonConnect2_OnClick(object sender, RoutedEventArgs e)
        {
            _videoReciever2 = new VideoReciever(Image2, "40405");
            await _videoReciever2.Initialize(Host2.Text);
            _videoReciever2.StartListening();
        }

    }
}
