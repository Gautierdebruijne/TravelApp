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
    class TaskViewModel
    {
        public List<TaskModel> Tasks;
        private HttpClient _client;

        public TaskViewModel()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            var token = "Bearer " + local.Values["token"];

            _client = new HttpClient(clientHandler);
            _client.DefaultRequestHeaders.Add("Authorization", token);

            Tasks = new List<TaskModel>();
        }

        public async Task GetAllTasks(int tripID)
        {
            var url = "https://localhost:5001/api/User/" + tripID + "/tasks";
            var json = await _client.GetStringAsync(url);
            var tasks = JsonConvert.DeserializeObject<ICollection<TaskModel>>(json);

            foreach (var t in tasks)
                Tasks.Add(t);
        }
    }
}
