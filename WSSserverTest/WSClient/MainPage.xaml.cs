using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
            var sws = new StreamSocket();
            await sws.ConnectAsync(new HostName("192.168.1.4"),"40403");
           
            StartSendHimData(sws.InputStream);

        }
        private async void StartSendHimData(IInputStream input)
        {
            var streamReader = input.AsStreamForRead();
            while (true)
            {
                await Task.Delay(500);
                var bytes = new byte[4];
                await streamReader.ReadAsync(bytes, 0, 4);
                var frameLengthInt =
                    BitConverter.ToInt32(bytes, 0);

                var frame = new byte[frameLengthInt];

                await streamReader.ReadAsync(frame, 0,frame.Length);
                Debug.WriteLine(frameLengthInt + "  " + frame.Length);
                await streamReader.FlushAsync();
                Image.Source = await ConvertBytesToBitmapImage(frame);
            }
        }
        private static async Task<BitmapImage> ConvertBytesToBitmapImage(byte[] bytes)
        {
            var image = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }
            return image;
        }


    }
}
