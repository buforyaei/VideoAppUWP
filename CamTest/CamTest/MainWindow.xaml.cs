using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;

namespace CamTest
{
    public partial class MainWindow
    {
        private VideoCaptureDevice _device = new VideoCaptureDevice();
        public FilterInfoCollection LoaclWebCamsCollection;
        private Socket _socket;
        private bool _ifShouldSend;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            LoaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _device = new VideoCaptureDevice(LoaclWebCamsCollection[0].MonikerString);
            
            _device.NewFrame += DeviceOnNewFrame;
            _device.Start();


        }
        private byte[] _frameInBytes;

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof (byte[]));
        }
        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Exception caught in process: " + ex);
                return false;
            }
        }
        private void DeviceOnNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var stream = new MemoryStream();
            eventArgs.Frame.SaveJpeg(stream, 50);
            var imageBytes = stream.GetBuffer();
            Dispatcher.Invoke(() =>
            {
                Image.Source = ByteToImageSource(imageBytes);
            });
            if (!_ifShouldSend) return;
            _frameInBytes = imageBytes;
            _socket.Send(new List<ArraySegment<byte>>
            {
                new ArraySegment<byte>(
                    BitConverter.GetBytes(_frameInBytes.Length))
            });
            _socket.Send(new List<ArraySegment<byte>>
            {
                new ArraySegment<byte>(_frameInBytes)
            });
        }

        public static ImageSource ByteToImageSource(byte[] imageData)
        {
            var biImg = new BitmapImage();
            var ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            var imgSrc = (ImageSource) biImg;
            return imgSrc;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            TryConnect();
        }

        private void TryConnect()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(IpTextBox.Text, int.Parse(PortTextBox.Text));
                _ifShouldSend = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(@"Exception caught in process: " + ex);
            }
        }
        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
    }
}
