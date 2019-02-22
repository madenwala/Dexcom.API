using Dexcom.Api;
using Dexcom.Api.Models;
using Dexcom.Api.Uwp;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Dexcom.Uwp
{
    public sealed partial class MainPage : Page
    {
        private static DateTime END_DATE = new DateTime(2018, 4, 15);

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            CancellationToken ct = new CancellationToken();

            var client = new DexcomClient(Credentials.CLIENT_ID, Credentials.CLIENT_SECRET, Credentials.CALLBACK_URL, new DexcomAuthProviderForWindows(), tsIsDev.IsOn);
            spToken.DataContext = await client.AuthenticateAsync(ct);
            //client.Token = this.Token;

            try
            {
                piDataRange.DataContext = null;
                piDataRange.DataContext = JsonConvert.SerializeObject(await client.GetDataRangeAsync(ct));
            }
            catch (Exception ex)
            {
                piDataRange.DataContext = ex.ToString();
            }

            try
            {
                piEGV.DataContext = null;
                piEGV.DataContext = JsonConvert.SerializeObject(await client.GetEstimatedGlucoseValueAsync(END_DATE, ct));
            }
            catch(Exception ex)
            {
                piEGV.DataContext = ex.ToString();
            }

            try
            {
                piDevices.DataContext = null;
                piDevices.DataContext = JsonConvert.SerializeObject(await client.GetDevicesAsync(END_DATE, ct));
            }
            catch (Exception ex)
            {
                piDevices.DataContext = ex.ToString();
            }

            try
            {
                piEvents.DataContext = null;
                piEvents.DataContext = JsonConvert.SerializeObject(await client.GetEventsAsync(END_DATE, ct));
            }
            catch (Exception ex)
            {
                piEvents.DataContext = ex.ToString();
            }

            try
            {
                piCalibrations.DataContext = null;
                piCalibrations.DataContext = JsonConvert.SerializeObject(await client.GetCalibrateAsync(END_DATE, ct));
            }
            catch (Exception ex)
            {
                piCalibrations.DataContext = ex.ToString();
            }

            try
            {
                //Statistics stats = new Statistics();
                //piStatistics.DataContext = JsonConvert.SerializeObject(await client.PostStatisticsAsync(stats, END_DATE, ct));
                piStatistics.DataContext = "Not yet implemented";
            }
            catch (Exception ex)
            {
                piStatistics.DataContext = ex.ToString();
            }
        }
    }
}
