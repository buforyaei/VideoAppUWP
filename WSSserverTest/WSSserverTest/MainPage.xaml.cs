using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
            var server = new SocketServer(40404);
            
            server.ServerStopped+=ServerOnServerStopped;
            server.ConnectionRequested = ConnectionRequested;
            server.Start();
            var port = server.Port;
        }


        private void ConnectionRequested(ISocketServer sender, string hostname, Stream input, Stream output)
        {
            Listen(input);
        }

        private async void Listen(Stream dr)
        {
            while (true)
            {
                var buffer = new byte[4];
                await dr.ReadAsync(buffer, 0,4);
                await dr.FlushAsync();
                await Task.Delay(500);
                Debug.WriteLine(buffer[0] + buffer[1] + buffer[2] + buffer[3] );
                
            }


        }
        private void ServerOnServerStopped(IServer server)
        {
            Debugger.Break();
        }
    }
}
