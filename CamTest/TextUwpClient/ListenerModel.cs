using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight;

namespace TextUwpClient
{
    public class ListenerModel : ObservableObject
    {
        private readonly StreamSocketListener _tcpListener;
        private StreamSocket _connectedSocket;
        private DataReader _dataReader;
        private ImageSource _imageSource;

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set { Set(ref _imageSource, value); }
        }

        public ListenerModel(string port)
        {
            _tcpListener = new StreamSocketListener();
            _tcpListener.ConnectionReceived += OnConnected;
            Task.Run(() => _tcpListener.BindEndpointAsync(null, port));
        }

        private void OnConnected(
            StreamSocketListener sender,
            StreamSocketListenerConnectionReceivedEventArgs args)
        {
            _connectedSocket = args.Socket;
            _dataReader = new DataReader(_connectedSocket.InputStream);
            StartListening();
        }

        public async void StartListening()
        {
            var frameSizeInBytes = new byte[4];
            while (true)
            {
                try
                {
                    await _dataReader.LoadAsync(4);
                    _dataReader.ReadBytes(frameSizeInBytes);

                    var frameLengthInt =
                        BitConverter.ToUInt32(frameSizeInBytes, 0);
                    Debug.WriteLine(frameLengthInt.ToString());

                    var frame = new byte[frameLengthInt];

                    await _dataReader.LoadAsync(frameLengthInt);
                    _dataReader.ReadBytes(frame);
                    Debug.WriteLine(frameLengthInt + "  " + frame.Length);
                    await 
                        CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,async() =>
                        {
                            ImageSource = await Utlis.ConvertBytesToBitmapImage(frame);
                        }
                    );
                   


                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                }


            }
        }
    }
}

