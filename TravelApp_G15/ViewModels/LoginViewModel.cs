using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TravelApp_G15.Models;
using Windows.Storage;

namespace TravelApp_G15.ViewModels
{
    public class LoginViewModel
    {
        public bool Success { get; set; } = false;
        private HttpClient _client;

        public LoginViewModel()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            var token = "Bearer " + local.Values["token"];

            _client = new HttpClient(clientHandler);
            _client.DefaultRequestHeaders.Add("Authorization", token);
        }

        public async Task<String> Login(string email, string password)
        {
            var login = new LoginModel {Email = email, Password = password };
            var loginJson = JsonConvert.SerializeObject(login);
            var url = "https://localhost:5001/api/User/Login";
            //var url = "https://travelappg15api.azurewebsites.net/api/User/Login";

            var data = new StringContent(loginJson, Encoding.UTF8, "application/json");
            var res = await _client.PostAsync(url, data);
            var json = await res.Content.ReadAsStringAsync();

            if (res.IsSuccessStatusCode)
            {
                ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
                local.Values["token"] = json.ToString();

                Success = true;
            }

            return res.Content.ToString();
        }
    }
}
