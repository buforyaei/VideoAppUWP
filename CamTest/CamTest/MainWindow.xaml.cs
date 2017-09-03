using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AForge.Video.DirectShow;

namespace CamTest
{
    public partial class MainWindow
    {

        private ObservableCollection<CameraModel> Cameras { get; set; }
        private ObservableCollection<Image> Images { get; set; }

       
        public MainWindow()
        {
            InitializeComponent();
            Cameras = new ObservableCollection<CameraModel>();
            Images = new ObservableCollection<Image>();
            ListView.ItemsSource = Images;
            Loaded += OnLoaded;
        }
       
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            InitializeCameraDevices();
        }

        private async void InitializeCameraDevices()
        {
            var loaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            for (var i = 0; i< loaclWebCamsCollection.Count; i++)
            {
                var data = await CameraWebServiceClient.Instance.GetConnectionData();
                if (data != null)
                {
                    var image = new Image();
                    Images.Add(image);
                    var cameraModel = new CameraModel(
                        new VideoCaptureDevice(loaclWebCamsCollection[i].MonikerString), image);
                    cameraModel.TryConnect(data.Ip,int.Parse(data.Port));
                    Cameras.Add(cameraModel);
                    await Task.Delay(500);
                } 
            }  
        }
        
        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }
    }
}
