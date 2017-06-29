using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AForge.Video;
using AForge.Video.DirectShow;

namespace CamTest
{
    public class CameraModel
    {
        private byte[] _frameInBytes;
        private bool _ifConnected;

        public Socket Socket { get; set; }
        public VideoCaptureDevice VideoDevice { get; set; }
        public Image ImageControl { get; set; }

        public CameraModel(VideoCaptureDevice videoDevice, Image imageControl)
        {
            VideoDevice = videoDevice;
            ImageControl = imageControl;

            VideoDevice.NewFrame += VideoDeviceOnNewFrame;
            VideoDevice.Start();
        }

        private void VideoDeviceOnNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var stream = new MemoryStream();
            eventArgs.Frame.SaveJpeg(stream, 50);
            var imageBytes = stream.GetBuffer();
            Application.Current.Dispatcher.Invoke(() =>
            {
                ImageControl.Source = Helpers.ByteToImageSource(imageBytes);
            });
            if (!_ifConnected) return;
            _frameInBytes = imageBytes;
            Socket.Send(new List<ArraySegment<byte>>
            {
                new ArraySegment<byte>(
                    BitConverter.GetBytes(_frameInBytes.Length))
            });
            Socket.Send(new List<ArraySegment<byte>>
            {
                new ArraySegment<byte>(_frameInBytes)
            });
        }

        public void TryConnect(string ip, int port)
        {
            try
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.Connect(ip, port);
                _ifConnected = true;
            }
            catch (Exception ex)
            {
                _ifConnected = false;
                Console.WriteLine(@"Exception caught in process: " + ex);
            }
        }


    }
}
