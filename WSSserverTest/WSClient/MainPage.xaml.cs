using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WSClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var sws = new StreamWebSocket();
            await sws.ConnectAsync(new Uri("ws://192.168.88.170:40404"));
            StartSendHimData(sws.OutputStream);

        }
        private async void StartSendHimData(IOutputStream output)
        {
            var random = new Random();

            while (true)
            {
                await Task.Delay(500);
                var bytes = new byte[4];
                Debug.WriteLine(bytes[0] + bytes[1] + bytes[2] + bytes[3]);
                random.NextBytes(bytes);
                await output.WriteAsync(bytes.AsBuffer());
                await output.FlushAsync();

            }
        }
       
    }
}
