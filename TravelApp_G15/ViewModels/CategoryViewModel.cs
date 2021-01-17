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
    class CategoryViewModel
    {
        public ObservableCollection<Category> Categories;
        private HttpClient _client;
        //private string _apiUrl = "https://travelappg15api.azurewebsites.net/api";
        private string _apiUrl = "https://localhost:5001/api";

        public CategoryViewModel()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            var token = "Bearer " + local.Values["token"];

            _client = new HttpClient(clientHandler);
            _client.DefaultRequestHeaders.Add("Authorization", token);

            Categories = new ObservableCollection<Category>();
        }

        public async Task GetAllCategories(int tripID)
        {
            var url = _apiUrl + "/User/" + tripID + "/categories";
            var json = await _client.GetStringAsync(url);
            var categories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(json);

            foreach (var c in categories)
                Categories.Add(c);
        }

        public async Task AddCategory(int tripID, string name)
        {
            var category = new Category { Name = name };
            var categorieJson = JsonConvert.SerializeObject(category);
            var url = _apiUrl + "/User/" + tripID + "/addCategory";

            var result = await _client.PostAsync(url, new StringContent(categorieJson, Encoding.UTF8, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                await GetAllCategories(tripID);
            }
        }

        public async Task DeleteCategory(int categoryID, int tripID)
        {
            var url = _apiUrl + "/User/" + tripID + "/Category/" + categoryID;

            var res = await _client.DeleteAsync(url);
        }
    }
}
