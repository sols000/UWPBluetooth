using Plugin.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace UWPBluetooth
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void OnMainPageLoaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("主窗口开始加载");

            AdapterStatus Astatus = CrossBleAdapter.Current.Status;
            CrossBleAdapter.Current.WhenStatusChanged().Subscribe(status =>
            {
                Debug.WriteLine("状态更新：" + status.ToString());
            });
            Debug.WriteLine("主窗口加载完成");
        }

        private void OnClicked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("普通扫描");

            var scanner = CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
            {
                // do something with it
                // the scanresult contains the device, RSSI, and advertisement packet
                Debug.WriteLine("扫描到设备：" + scanResult.Device.Uuid + "扫描到设备：" + scanResult.Device.Name);
            });
            //scanner.Dispose(); // to stop scanning
        }
        IDevice currdev = null;
        private void OnScanClicked2(object sender, RoutedEventArgs e)
        {

            CrossBleAdapter.Current.Scan(new ScanConfig
            {
                //ScanType = BleScanType.Background,
                ServiceUuids = { new Guid("48EB9001-F352-5FA0-9B06-8FCAA22602CF") }
            }).Subscribe(scanResult =>
            {

                this.Invoke(() =>
                {
                    TXT.Text = "扫描到设备：" + scanResult.Device.Name;
                });

                currdev = scanResult.Device;

            });

        }
        public async void Invoke(Action action, Windows.UI.Core.CoreDispatcherPriority Priority = Windows.UI.Core.CoreDispatcherPriority.Normal)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Priority, () => { action(); });

        }
       


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //currdev.BeginReliableWriteTransaction().Write()
            IObservable<IGattCharacteristic> array = currdev.GetCharacteristicsForService(new Guid("48EB9002-F352-5FA0-9B06-8FCAA22602CF"));
            // TXT.Text = array.ToString();

            //  currdev.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
            //{
            //      // read, write, or subscribe to notifications here
            //      var result = characteristic.Read(); // use result.Data to see response
            //                                          //await characteristic.Write(bytes);

            //      characteristic.EnableNotifications();
            //    characteristic.WhenNotificationReceived().Subscribe(result1 =>
            //    {
            //          //result.Data to get at response
            //          TXT.Text = result1.Characteristic.Value.ToString();
            //    });
            //});
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            currdev.Connect();
           
        }
    }
}
