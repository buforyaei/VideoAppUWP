using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace VideoStreamApp
{
    class VideoProvider
    {
        private MediaCapture _captureElement;
        private readonly StreamSocketListener _tcpListener;
        private StreamSocket _connectedSocket;
        private DataWriter _dataWriter;
        private string _port;
        private IRandomAccessStream _stream;
        private TextBlock Tb;
        public VideoProvider(TextBlock tb, string port )
        {
            _port = port;
            Tb = tb;
            _tcpListener = new StreamSocketListener();
            _tcpListener.ConnectionReceived += OnConnected;
            _captureElement = new MediaCapture();
            
            _stream = new InMemoryRandomAccessStream();
        }

        public async Task InitializeProvider()
        {
            await _tcpListener.BindEndpointAsync(null, _port);
            _captureElement = new MediaCapture();
            await _captureElement.InitializeAsync(
                new MediaCaptureInitializationSettings());

        }
        private void OnConnected(
             StreamSocketListener sender,
             StreamSocketListenerConnectionReceivedEventArgs args)
        {

            _connectedSocket = args.Socket;
            _dataWriter = new DataWriter(_connectedSocket.OutputStream);
            RunStream();
        }

        private async void RunStream()
        {
            while (true)
            {
                try
                {
                    _stream = new InMemoryRandomAccessStream();
                    await _captureElement.CapturePhotoToStreamAsync(
                    ImageEncodingProperties.CreateJpeg(), _stream);
                    
                    var size = BitConverter.GetBytes((ushort)_stream.Size);
                    _dataWriter.WriteBytes(size);
                    await _dataWriter.StoreAsync();

                    Debug.WriteLine(_stream.Size.ToString());

                    _dataWriter.WriteBytes(ToByteArray(_stream.AsStream()));
                    await _dataWriter.StoreAsync();
                    await Task.Delay(50);
                }
                catch
                {
                    
                }
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
    }
}
