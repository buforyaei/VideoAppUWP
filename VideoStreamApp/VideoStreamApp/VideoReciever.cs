using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace VideoStreamApp
{
    public class VideoReciever
    {
        private readonly StreamSocket _socket;
        private string _port;
        private readonly Image _image;
        //private readonly DataWriter _dataWriter;
        private readonly DataReader _dataReader;
        public VideoReciever(Image image, string port)
        {
            _port = port;
            _image = image;
            _socket = new StreamSocket();
            _dataReader = new DataReader(_socket.InputStream);
            //_dataWriter = new DataWriter(_socket.OutputStream);
        }
        public async Task Initialize(string host)
        {
            await _socket.ConnectAsync(new HostName(host), _port);
        }

        public async void StartListening()
        {

            var frameSizeInBytes = new byte[2];
            while (true)
            {
                try
                {
                    await _dataReader.LoadAsync(2);
                    _dataReader.ReadBytes(frameSizeInBytes);

                    var frameLengthInt =
                        BitConverter.ToUInt16(frameSizeInBytes, 0);
                    Debug.WriteLine(frameLengthInt.ToString());

                    var frame = new byte[frameLengthInt];

                    await _dataReader.LoadAsync(frameLengthInt);
                    _dataReader.ReadBytes(frame);
                    Debug.WriteLine(frameLengthInt + "  " + frame.Length);
                    _image.Source = await ConvertBytesToBitmapImage(frame);
                    //await Task.Delay(50);
                }
                catch
                {
                    
                }


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
