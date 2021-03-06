﻿using Newtonsoft.Json;
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
    class ItemViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Item> CategoryItems { get; set; }
        private HttpClient _client;
        private string _apiUrl = "https://travelappg15api.azurewebsites.net/api";
        //private string _apiUrl = "https://localhost:5001/api";

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

            Items = new ObservableCollection<Item>();
            CategoryItems = new ObservableCollection<Item>();
        }

        #region Get
        public async Task GetAllItems(int tripID)
        {
            var url = _apiUrl + "/User/" + tripID + "/items";
            var json = await _client.GetStringAsync(url);
            var items = JsonConvert.DeserializeObject<ObservableCollection<Item>>(json);

            Items.Clear();

            foreach (var i in items)
                Items.Add(i);
        }

        public async Task GetItemsByCategorie(int tripID, int categorieID)
        {
            var url = _apiUrl + "/User/" + tripID + "/Category/" + categorieID + "/items";
            var json = await _client.GetStringAsync(url);
            var items = JsonConvert.DeserializeObject<ObservableCollection<Item>>(json);

            CategoryItems.Clear();

            foreach (var i in items)
                CategoryItems.Add(i);
        }
        #endregion

        #region Post
        public async Task AddItem(int tripID, String name, int amount)
        {
            var item = new Item { Name = name, Amount = amount, Checked = false, Category = null };
            var itemJson = JsonConvert.SerializeObject(item);
            var url = _apiUrl + "/User/" + tripID + "/addItem";


            var res = await _client.PostAsync(url, new StringContent(itemJson, Encoding.UTF8, "application/json"));

            if (res.IsSuccessStatusCode)
            {
                await GetAllItems(tripID);
            }
        }

        public async Task ChangeItem(Item item)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());

            var itemJson = JsonConvert.SerializeObject(item);
            var url = _apiUrl + "/User/" + tripID + "/Item/" + item.ItemID;

            var res = await _client.PutAsync(url, new StringContent(itemJson, Encoding.UTF8, "application/json"));
        }


        public async Task AddItemToCategory(int tripID, int categorieID, int itemID)
        {
            var url = _apiUrl + "/User/" + tripID + "/" + categorieID + "/Categorie/" + itemID + "/addItemToCategory";
            var result = await _client.PostAsync(url, null);
        }
        #endregion

        #region Put
        public async Task ChangeItem(int itemID, int tripID)
        {
            var url = _apiUrl + "/User/" + tripID + "/Task/" + itemID;

            var res = await _client.PutAsync(url, null);
        }
        #endregion

        #region Delete
        public async Task DeleteItem(int tripID, int itemID)
        {
            var url = _apiUrl + "/User/" + tripID + "/Items/" + itemID;

            var result = await _client.DeleteAsync(url);
        }
        #endregion
    }
}
