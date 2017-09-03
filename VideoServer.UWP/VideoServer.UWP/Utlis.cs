using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Networking.Connectivity;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;

namespace VideoServer.UWP
{
    public static class Utlis
    {
        public static async Task<BitmapImage> ConvertBytesToBitmapImage(byte[] bytes)
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

        public static string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return null;
            var hostname =
                NetworkInformation.GetHostNames()
                    .SingleOrDefault(
                        hn =>
                            hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                            == icp.NetworkAdapter.NetworkAdapterId);

            return hostname?.CanonicalName;
        }

        public static async void ShowStatusBar()
        {
            if (!ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar")) return;
            var statusbar = StatusBar.GetForCurrentView();
            statusbar.BackgroundColor = Colors.Black;
            statusbar.BackgroundOpacity = 1;
            statusbar.ForegroundColor = Colors.White;
            await statusbar.ShowAsync();
        }
    }
}
