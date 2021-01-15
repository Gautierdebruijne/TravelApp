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
    class TripViewModel
    {
        public ICollection<Trip> Trips;
        private HttpClient _client;

        public TripViewModel()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            var token = "Bearer" + local.Values["token"];

            _client = new HttpClient(clientHandler);
            _client.DefaultRequestHeaders.Add("Authorization", token);

            Trips = new List<Trip>();
        }

        public async Task GetAllTrips(string userID)
        {
            var url = "https://localhost:5001/api/User/" + userID + "/trips";
            var json = await _client.GetStringAsync(url);
            var trips = JsonConvert.DeserializeObject<ICollection<Trip>>(json);

            foreach(var t in trips)
            {
                Trips.Add(t);
            }
        }
    }
}
