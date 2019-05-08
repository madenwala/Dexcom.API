using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Dexcom.Uwp
{
    public sealed partial class MainPage : Page
    {
        private WinkClient Client { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            CancellationToken ct = new CancellationToken();
            await this.AuthenticateAsync(ct);
            await this.RefreshAsync(ct);
        }

        private async void btnRefreshToken_Click(object sender, RoutedEventArgs e)
        {
            CancellationToken ct = new CancellationToken();
            if (this.Client?.Token != null)
            {
                spToken.DataContext = await this.Client.RefreshAccessTokenAsync(ct);
                await this.RefreshAsync(ct);
            }
            else
                await this.AuthenticateAsync(ct);
        }

        private async void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (this.Client != null)
            {
                CancellationToken ct = new CancellationToken();
                spToken.DataContext = this.Client = null;
                await this.RefreshAsync(ct);
            }
        }

        private async void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            CancellationToken ct = new CancellationToken();
            if (this.Client == null)
                await this.AuthenticateAsync(ct);
            await this.RefreshAsync(ct);
        }

        private async Task AuthenticateAsync(CancellationToken ct)
        {
            this.Client = new DexcomClient(Credentials.CLIENT_ID, Credentials.CLIENT_SECRET, Credentials.CALLBACK_URL, new DexcomAuthProviderForWindows(), tsIsDev.IsOn);
            spToken.DataContext = await this.Client.AuthenticateAsync(ct);
        }

        private async Task RefreshAsync(CancellationToken ct)
        {
            if(this.Client?.Token == null)
            {
                piDataRange.DataContext = null;
                piEGV.DataContext = null;
                piDevices.DataContext = null;
                piEvents.DataContext = null;
                piCalibrations.DataContext = null;
                piStatistics.DataContext = null;
                return;
            }

            try
            {
                piDataRange.DataContext = null;
                piDataRange.DataContext = JsonConvert.SerializeObject(await this.Client.GetDataRangeAsync(ct));
            }
            catch (Exception ex)
            {
                piDataRange.DataContext = ex.ToString();
            }

            try
            {
                piEGV.DataContext = null;
                piEGV.DataContext = JsonConvert.SerializeObject(await this.Client.GetEstimatedGlucoseValueAsync(dpStartDate.SelectedDate.Value.Date, dpEndDate.SelectedDate.Value.Date, ct));
            }
            catch (Exception ex)
            {
                piEGV.DataContext = ex.ToString();
            }

            try
            {
                piDevices.DataContext = null;
                piDevices.DataContext = JsonConvert.SerializeObject(await this.Client.GetDevicesAsync(dpStartDate.SelectedDate.Value.Date, dpEndDate.SelectedDate.Value.Date, ct));
            }
            catch (Exception ex)
            {
                piDevices.DataContext = ex.ToString();
            }

            try
            {
                piEvents.DataContext = null;
                piEvents.DataContext = JsonConvert.SerializeObject(await this.Client.GetEventsAsync(dpStartDate.SelectedDate.Value.Date, dpEndDate.SelectedDate.Value.Date, ct));
            }
            catch (Exception ex)
            {
                piEvents.DataContext = ex.ToString();
            }

            try
            {
                piCalibrations.DataContext = null;
                piCalibrations.DataContext = JsonConvert.SerializeObject(await this.Client.GetCalibrateAsync(dpStartDate.SelectedDate.Value.Date, dpEndDate.SelectedDate.Value.Date, ct));
            }
            catch (Exception ex)
            {
                piCalibrations.DataContext = ex.ToString();
            }

            try
            {
                //Statistics stats = new Statistics();
                //piStatistics.DataContext = JsonConvert.SerializeObject(await client.PostStatisticsAsync(stats, dpStartDate.SelectedDate.Value.Date, dpEndDate.SelectedDate.Value.Date, ct));
                piStatistics.DataContext = "Not yet implemented";
            }
            catch (Exception ex)
            {
                piStatistics.DataContext = ex.ToString();
            }
        }

        private void TsIsDev_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (tsIsDev.IsOn)
            {
                dpStartDate.SelectedDate = new DateTimeOffset(new DateTime(2018, 4, 15));
                dpEndDate.SelectedDate = new DateTimeOffset(new DateTime(2018, 4, 15));
            }
            else
            {
                dpStartDate.SelectedDate = new DateTimeOffset(DateTime.Now);
                dpEndDate.SelectedDate = new DateTimeOffset(DateTime.Now.AddDays(1));
            }
        }
    }
}
