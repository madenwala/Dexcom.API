﻿using Dexcom.Api.Extensions;
using Dexcom.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Dexcom.Api
{
    public sealed class DexcomClient : ClientBase
    {
        #region Parameters

        private string ClientID { get; }
        private string ClientSecret { get; }
        private string CallbackUrl { get; }
        private IDexcomAuthProvider AuthProvider { get; }
        public bool IsDevelopment { get; private set; } = false;
        public AuthenticationResult Token { get; set; }

        #endregion

        #region Constructors

        public DexcomClient(string clientID, string clientSecret, string callbackUrl, IDexcomAuthProvider authProvider, bool isDevelopment = false) : base()
        {
            if (string.IsNullOrEmpty(clientID)) throw new ArgumentNullException(nameof(clientID));
            if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
            if (string.IsNullOrEmpty(callbackUrl)) throw new ArgumentNullException(nameof(callbackUrl));

            this.ClientID = clientID;
            this.ClientSecret = clientSecret;
            this.CallbackUrl = callbackUrl;
            this.AuthProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
            this.SetEnvironment(isDevelopment);
        }

        #endregion

        #region Methods

        #region Private

        private const string AUTHORIZATION = "authorization";

        private void SetHeaders()
        {
            this.Client.DefaultRequestHeaders.Clear();
            this.Client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            this.Client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrEmpty(this.Token?.AccessToken))
            {
                this.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                this.Client.DefaultRequestHeaders.Add(AUTHORIZATION, $"{this.Token.TokenType} {this.Token.AccessToken}");
            }
            else
            {
                if(this.Client.DefaultRequestHeaders.Contains(AUTHORIZATION))
                    this.Client.DefaultRequestHeaders.Remove(AUTHORIZATION);
            }
        }

        private void SetEnvironment(bool isDevelopment)
        {
            this.IsDevelopment = isDevelopment;
            if (this.IsDevelopment)
                this.SetBaseUrl("https://sandbox-api.dexcom.com");
            else
                this.SetBaseUrl("https://api.dexcom.com");
        }

        private string FormatDate(DateTime dt)
        {
            // Format as 2017-06-16T15:30:00
            return dt.ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss");
        }

        private void ValidateDates(ref DateTime startDate, ref DateTime endDate)
        {
            if(startDate > endDate)
            {
                var tmp = endDate;
                endDate = startDate;
                startDate = tmp;
            }

            if (startDate < endDate.AddMonths(-3))
                startDate = endDate.AddMonths(-3);
        }

        #endregion

        #region Public

        #region Authentication

        public async Task<AuthenticationResult> AuthenticateAsync(CancellationToken ct)
        {
            if(this.Token != null)
            {
                this.Token = await this.RefreshAccessTokenAsync(ct);
                if (this.Token != null)
                    return this.Token;
            }

            var url = this.GetUserAuthorizationUrl(this.CallbackUrl);
            var authorizationCode = await this.AuthProvider.PromptForCredentialsAsync(url, this.CallbackUrl, ct);
            if (authorizationCode != null)
                this.Token = await this.GetAccessTokenAsync(authorizationCode, this.CallbackUrl, ct);
            else
                this.Token = null;

            return this.Token;
        }

        private async Task<AuthenticationResult> RefreshAccessTokenAsync(CancellationToken ct)
        {
            // TODO implement refresh token
            return await Task.FromResult<AuthenticationResult>(null);
        }

        private string GetUserAuthorizationUrl(string redirectUrl, string state = null)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("client_id", this.ClientID);
            dic.Add("redirect_uri", redirectUrl);
            dic.Add("response_type", "code");
            dic.Add("scope", "offline_access");
            if(state != null) dic.Add("state", state);
            return new Uri(this.BaseUri, "/v2/oauth2/login" + dic.ToQueryString()).ToString();
        }

        private async Task<AuthenticationResult> GetAccessTokenAsync(string authorizationCode, string redirectUrl, CancellationToken ct)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("client_id", this.ClientID);
            dic.Add("client_secret", this.ClientSecret);
            dic.Add("code", authorizationCode);
            dic.Add("grant_type", "authorization_code");
            dic.Add("redirect_uri", redirectUrl);

            this.Client.DefaultRequestHeaders.Clear();
            this.Client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            this.Token = await this.PostAsync<AuthenticationResult>("/v2/oauth2/token", ct, new FormUrlEncodedContent(dic));
            return this.Token;
        }

        #endregion

        #region DataRange

        public async Task<string> GetDataRangeAsync(CancellationToken ct)
        {
            this.SetHeaders();
            using (var response = await this.GetAsync("/v2/users/self/dataRange", ct))
            {
                response.EnsureSuccessStatusCode();
                return await response.ContentToString();
            }
        }

        #endregion DataRange

        #region EGV

        public Task<string> GetEstimatedGlucoseValueAsync(CancellationToken ct)
        {
            // Return EGV for based on today
            return this.GetEstimatedGlucoseValueAsync(DateTime.Now, ct);
        }

        public Task<string> GetEstimatedGlucoseValueAsync(DateTime endDate, CancellationToken ct)
        {
            // Return the last 24 hours from the end date
            return this.GetEstimatedGlucoseValueAsync(endDate, 24, ct);
        }

        public Task<string> GetEstimatedGlucoseValueAsync(DateTime endDate, int hours, CancellationToken ct)
        {
            // Ensure that hours is negative
            if (hours > 0)
                hours = -hours;
            else if (hours == 0)
                hours = -3;
            return this.GetEstimatedGlucoseValueAsync(endDate.AddHours(hours), endDate, ct);
        }

        public async Task<string> GetEstimatedGlucoseValueAsync(DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            this.SetHeaders();

            this.ValidateDates(ref startDate, ref endDate);
            var dic = new Dictionary<string, object>();
            dic.Add("startDate", this.FormatDate(startDate));
            dic.Add("endDate", this.FormatDate(endDate));

            using (var response = await this.GetAsync("/v2/users/self/egvs" + dic.ToQueryString(), ct))
            {
                response.EnsureSuccessStatusCode();
                return await response.ContentToString();
            }
        }

        #endregion EGV

        #region Devices

        public Task<string> GetDevicesAsync(CancellationToken ct)
        {
            // Return devices for based on today
            return this.GetDevicesAsync(DateTime.Now, ct);
        }

        public Task<string> GetDevicesAsync(DateTime endDate, CancellationToken ct)
        {
            // Return the last 24 hours from the end date
            return this.GetDevicesAsync(endDate, 24, ct);
        }

        public Task<string> GetDevicesAsync(DateTime endDate, int hours, CancellationToken ct)
        {
            // Ensure that hours is negative
            if (hours > 0)
                hours = -hours;
            else if (hours == 0)
                hours = -3;
            return this.GetDevicesAsync(endDate.AddHours(hours), endDate, ct);
        }

        public async Task<string> GetDevicesAsync(DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            this.SetHeaders();

            this.ValidateDates(ref startDate, ref endDate);
            var dic = new Dictionary<string, object>();
            dic.Add("startDate", this.FormatDate(startDate));
            dic.Add("endDate", this.FormatDate(endDate));

            using (var response = await this.GetAsync("/v2/users/self/devices" + dic.ToQueryString(), ct))
            {
                response.EnsureSuccessStatusCode();
                return await response.ContentToString();
            }
        }

        #endregion

        #region Events

        public Task<EventsResponse> GetEventsAsync(CancellationToken ct)
        {
            // Return Events for based on today
            return this.GetEventsAsync(DateTime.Now, ct);
        }

        public Task<EventsResponse> GetEventsAsync(DateTime endDate, CancellationToken ct)
        {
            // Return the last 24 hours from the end date
            return this.GetEventsAsync(endDate, 24, ct);
        }

        public Task<EventsResponse> GetEventsAsync(DateTime endDate, int hours, CancellationToken ct)
        {
            // Ensure that hours is negative
            if (hours > 0)
                hours = -hours;
            else if (hours == 0)
                hours = -3;
            return this.GetEventsAsync(endDate.AddHours(hours), endDate, ct);
        }

        public async Task<EventsResponse> GetEventsAsync(DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            this.SetHeaders();

            this.ValidateDates(ref startDate, ref endDate);
            var dic = new Dictionary<string, object>();
            dic.Add("startDate", this.FormatDate(startDate));
            dic.Add("endDate", this.FormatDate(endDate));

            return await this.GetAsync<EventsResponse>("/v2/users/self/events" + dic.ToQueryString(), ct);
        }

        #endregion

        #region Calibrate

        public Task<string> GetCalibrateAsync(CancellationToken ct)
        {
            // Return calibrate for based on today
            return this.GetCalibrateAsync(DateTime.Now, ct);
        }

        public Task<string> GetCalibrateAsync(DateTime endDate, CancellationToken ct)
        {
            // Return the last 24 hours from the end date
            return this.GetCalibrateAsync(endDate, 24, ct);
        }

        public Task<string> GetCalibrateAsync(DateTime endDate, int hours, CancellationToken ct)
        {
            // Ensure that hours is negative
            if (hours > 0)
                hours = -hours;
            else if (hours == 0)
                hours = -3;
            return this.GetCalibrateAsync(endDate.AddHours(hours), endDate, ct);
        }

        public async Task<string> GetCalibrateAsync(DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            this.SetHeaders();

            this.ValidateDates(ref startDate, ref endDate);
            var dic = new Dictionary<string, object>();
            dic.Add("startDate", this.FormatDate(startDate));
            dic.Add("endDate", this.FormatDate(endDate));

            using (var response = await this.GetAsync("/v2/users/self/calibrations" + dic.ToQueryString(), ct))
            {
                response.EnsureSuccessStatusCode();
                return await response.ContentToString();
            }
        }

        #endregion

        #region Statistics

        public Task<string> PostStatisticsAsync(Statistics stats, CancellationToken ct)
        {
            // Return calibrate for based on today
            return this.PostStatisticsAsync(stats, DateTime.Now, ct);
        }

        public Task<string> PostStatisticsAsync(Statistics stats, DateTime endDate, CancellationToken ct)
        {
            // Return the last 24 hours from the end date
            return this.PostStatisticsAsync(stats, endDate, 24, ct);
        }

        public Task<string> PostStatisticsAsync(Statistics stats, DateTime endDate, int hours, CancellationToken ct)
        {
            // Ensure that hours is negative
            if (hours > 0)
                hours = -hours;
            else if (hours == 0)
                hours = -3;
            return this.PostStatisticsAsync(stats, endDate.AddHours(hours), endDate, ct);
        }

        public async Task<string> PostStatisticsAsync(Statistics stats, DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            this.SetHeaders();

            this.ValidateDates(ref startDate, ref endDate);
            var dic = new Dictionary<string, object>();
            dic.Add("startDate", this.FormatDate(startDate));
            dic.Add("endDate", this.FormatDate(endDate));

            StringContent content = new StringContent(JsonConvert.SerializeObject(stats));
            using (var response = await this.PostAsync("/v2/users/self/calibrations" + dic.ToQueryString(), ct, content))
            {
                response.EnsureSuccessStatusCode();
                return await response.ContentToString();
            }
        }

        #endregion

        #endregion Public

        #endregion Methods
    }
}