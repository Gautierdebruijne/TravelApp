using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using TravelApp_G15.Models;
using Newtonsoft.Json;

namespace TravelApp_G15.ViewModels
{
    class LocationViewModel
    {
        public ObservableCollection<Location> Locations { get; set; }
        private HttpClient _client;

        public LocationViewModel()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            var token = "Bearer " + local.Values["token"];

            _client = new HttpClient(clientHandler);
            _client.DefaultRequestHeaders.Add("Authorization", token);

            Locations = new ObservableCollection<Location>();
        }

        public async Task GetLocation(int tripID)
        {
            var url = "https://localhost:5001/api/User/" + tripID + "/locations";
            var json = await _client.GetStringAsync(url);
            var items = JsonConvert.DeserializeObject<ObservableCollection<Location>>(json);

            Locations.Clear();

            foreach (var i in items)
                Locations.Add(i);
        }

        public async Task AddLocation(int tripID, string country, string city)
        {
            var location = new Location { Country = country, City = city };
            var locationJson = JsonConvert.SerializeObject(location);
            var url = "https://localhost:5001/api/User/" + tripID + "/addLocation";


            var result = await _client.PostAsync(url, new StringContent(locationJson, Encoding.UTF8, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                await GetLocation(tripID);
            }
        }

        public async Task DeleteLocation(int tripID, int locationID)
        {
            var url = "https://localhost:5001/api/User/" + tripID + "/Location/" + locationID;

            var result = await _client.DeleteAsync(url);
        }
    }
}
