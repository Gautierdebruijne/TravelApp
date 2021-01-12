using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public async Task<String> Login(string email, string password)
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            var client = new HttpClient(clientHandler);

            var login = new LoginModel(email, password);
            var jsonLogin = JsonConvert.SerializeObject(login);
            var url = "https://localhost:5001/api/User/Login";
            var data = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
            var res = await client.PostAsync(url, data);
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
