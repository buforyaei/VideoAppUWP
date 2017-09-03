using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using IotWeb.Common;
using IotWeb.Server;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WSSserverTest
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
            var server = new SocketServer(40403);
            server.ServerStopped += ServerOnServerStopped;
            server.ConnectionRequested = ConnectionRequested;
            server.Start();
            _captureElement = new MediaCapture();
            await _captureElement.InitializeAsync(
                new MediaCaptureInitializationSettings());
        }
        
        private void ConnectionRequested(ISocketServer sender, string hostname, Stream input, Stream output)
        {
            HostStream(output);
        }

        private InMemoryRandomAccessStream _stream;
        private MediaCapture _captureElement;
        private async void HostStream(Stream os)
        {
            
            while (true)
            {
                _stream = new InMemoryRandomAccessStream();
                await _captureElement.CapturePhotoToStreamAsync(
                ImageEncodingProperties.CreateJpeg(), _stream);

                var size = BitConverter.GetBytes((int)_stream.Size);
                await os.WriteAsync(size,0,size.Length);
                var byteArray = ToByteArray(_stream.AsStream());
                await os.WriteAsync(byteArray,0,byteArray.Length);
               
                await Task.Delay(50);
                Debug.WriteLine(byteArray.Length + " " + size);
            }
        }

        public static byte[] ToByteArray(Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            return buffer;
        }

        private void ServerOnServerStopped(IServer server)
        {
            Debugger.Break();
        }
    }
}
