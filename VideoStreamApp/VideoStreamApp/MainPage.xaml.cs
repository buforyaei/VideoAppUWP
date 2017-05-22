using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private VideoReciever _videoReciever;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ButtonConnect_OnClick(object sender, RoutedEventArgs e)
        {
            _videoReciever = new VideoReciever(Image);
            await _videoReciever.Initialize(Host.Text);
            _videoReciever.StartListening();
        }
        private async void ButtonHost_OnClick(object sender, RoutedEventArgs e)
        {
            _videoProvider = new VideoProvider(TextBlock);
            await _videoProvider.InitializeProvider();
            //long s = 15040;
            //var bytes = new byte[2];
            //bytes = BitConverter.GetBytes((ushort)s);
            //var number = BitConverter.ToUInt16(bytes,0);
        }
        
    }
}
