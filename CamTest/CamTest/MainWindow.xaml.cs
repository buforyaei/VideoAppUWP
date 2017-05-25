using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;
using Image = System.Drawing.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;

namespace CamTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
            

        }
        private byte[] _frameInBytes;

        public static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(img, typeof (byte[]));
        }

        private void DeviceOnNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (_ifShouldSend)
            {
                var imageBytes = ImageToByte(eventArgs.Frame);
                _frameInBytes = imageBytes;
                _socket.Send(new List<ArraySegment<byte>>
                {
                    new ArraySegment<byte>(
                        BitConverter.GetBytes((int)_frameInBytes.Length))
                });
                _socket.Send(new List<ArraySegment<byte>>
                {
                    new ArraySegment<byte>(_frameInBytes)
                });
            }
            Dispatcher.Invoke(() =>
            {
                Image.Source = BitmapToImageSource(eventArgs.Frame);
            });
        }
      
        private static BitmapImage BitmapToImageSource(Image bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _device.Start();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect("192.168.0.67",40404);
            _ifShouldSend = true;
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
    }
}
