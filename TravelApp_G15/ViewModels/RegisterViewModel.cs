using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TravelApp_G15.Models;
using Windows.Storage;


namespace TravelApp_G15.ViewModels
{
    class RegisterViewModel
    {
        public bool Success { get; set; } = false;

        public async Task<String> Register(string name, string email, string password)
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            var client = new HttpClient(clientHandler);

            var register = new Register(name, email, password);
            var jsonRegister = JsonConvert.SerializeObject(register);
            var url = "https://localhost:5001/api/User/Register";
            //var url = "https://travelappg15api.azurewebsites.net/api/User/Register";
            var data = new StringContent(jsonRegister, Encoding.UTF8, "application/json");
            string content = await data.ReadAsStringAsync();
            Debug.WriteLine(content);
            Debug.WriteLine("Ik geraak tot hier");
            var res = await client.PostAsync(url, data);
            var json = await res.Content.ReadAsStringAsync();

            if (res.IsSuccessStatusCode)
            {
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["token"] = json.ToString();
                Success = true;
            }
            else
            {
                Success = false;
            }
            Debug.WriteLine("Ik geraak zelf tot hier");
            return res.Content.ToString();
        }
    }
}