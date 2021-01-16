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
            var token = "Bearer " + local.Values["token"];

            _client = new HttpClient(clientHandler);
            _client.DefaultRequestHeaders.Add("Authorization", token);

            Trips = new List<Trip>();
        }

        public async Task GetAllTrips()
        {
            var url = "https://localhost:5001/api/User/trips";
            var json = await _client.GetStringAsync(url);
            var trips = JsonConvert.DeserializeObject<ICollection<Trip>>(json);

            foreach(var t in trips)
            {
                Trips.Add(t);
            }
        }

        public async Task AddTrip(string name, DateTime date)
        {
            var url = "https://localhost:5001/api/User/addTrip";
            var trip = new Trip { Name = name, Date = date };
            var tripJson = JsonConvert.SerializeObject(trip);

            var result = await _client.PostAsync(url, new StringContent(tripJson, Encoding.UTF8, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                await GetAllTrips();
            }
        }

        public async Task DeleteTrip(int tripID)
        {
            var url = "https://localhost:5001/api/Trip/tripID?tripID=" + tripID;
            var result = await _client.DeleteAsync(url);

            if (result.IsSuccessStatusCode)
            {
                await GetAllTrips();
            }
        }
    }
}
