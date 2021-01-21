using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Trip> Trips;
        private HttpClient _client;
        //private string _apiUrl = "https://travelappg15api.azurewebsites.net/api";
        private string _apiUrl = "https://localhost:5001/api";

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

            Trips = new ObservableCollection<Trip>();
        }

        public async Task GetAllTrips()
        {
            var url = _apiUrl + "/User/trips";
            var json = await _client.GetStringAsync(url);
            var trips = JsonConvert.DeserializeObject<ObservableCollection<Trip>>(json);

            Trips.Clear();

            foreach(var t in trips)
            {
                Trips.Add(t);
            }
        }

        public async Task AddTrip(string name, string country, string city, DateTime date)
        {
            var url = _apiUrl + "/User/addTrip";

            var location = new Location { Country = country, City = city };
            ICollection<Location> locations = new List<Location>();
            locations.Add(location);

            var trip = new Trip { Name = name, Locations = locations, Date = date };
            var tripJson = JsonConvert.SerializeObject(trip);

            var result = await _client.PostAsync(url, new StringContent(tripJson, Encoding.UTF8, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                await GetAllTrips();
            }
        }

        public async Task DeleteTrip(int tripID)
        {
            var url = _apiUrl + "/Trip/tripID?tripID=" + tripID;
            var result = await _client.DeleteAsync(url);

            if (result.IsSuccessStatusCode)
            {
                await GetAllTrips();
            }
        }
    }
}
