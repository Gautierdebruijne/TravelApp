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
    class ItemViewModel
    {
        public List<Item> Items;
        private HttpClient _client;

        public ItemViewModel()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            var token = "Bearer " + local.Values["token"];

            _client = new HttpClient(clientHandler);
            _client.DefaultRequestHeaders.Add("Authorization", token);

            Items = new List<Item>();
        }

        #region Get
        public async Task GetAllItems(int tripID)
        {
            var url = "https://localhost:5001/api/User/" + tripID + "/items";
            var json = await _client.GetStringAsync(url);
            var items = JsonConvert.DeserializeObject<ICollection<Item>>(json);

            foreach (var i in items)
                Items.Add(i);
        }

        /*public async Task GetItemsByCategorie(int tripID, int categorieID)
        {
            //TODO
        }*/
        #endregion

        #region Post
        public async Task AddItem(int tripID, String name, int amount)
        {
            var item = new Item { Name = name, Amount = amount, Checked = false, Category = null };
            var itemJson = JsonConvert.SerializeObject(item);
            var url = "https://localhost:5001/api/User/" + tripID + "/addItem";


            var res = await _client.PostAsync(url, new StringContent(itemJson, Encoding.UTF8, "application/json"));

            if (res.IsSuccessStatusCode)
            {
                await GetAllItems(tripID);
            }
        }

        public async Task AddItemToCategory(int tripID, int categorieID, int itemID)
        {
            var url = "https://localhost:5001/api/User/" + tripID + "/" + categorieID + "/Categorie/" + itemID + "/addItemToCategory";
            var result = await _client.PostAsync(url, null);
        }
        #endregion

        #region Put
        public async Task ChangeItem(int itemID, int tripID)
        {
            var url = "https://localhost:5001/api/User/" + tripID + "/Task/" + itemID;

            var res = await _client.PutAsync(url, null);
        }
        #endregion

        #region Delete
        public async Task DeleteItem(int tripID, int itemID)
        {
            var url = "https://localhost:5001/api/User/" + tripID + "/Items/" + itemID;

            var result = await _client.DeleteAsync(url);
        }
        #endregion
    }
}
