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
        int tripID;

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

            ApplicationDataContainer localSettings2 = Windows.Storage.ApplicationData.Current.LocalSettings;
            tripID = Int32.Parse(localSettings2.Values["tripID"].ToString());
        }

        public async Task GetAllTasks(int tripID)
        {
            var url = "https://localhost:5001/api/User/" + tripID + "/tasks";
            var json = await _client.GetStringAsync(url);
            var tasks = JsonConvert.DeserializeObject<ICollection<TaskModel>>(json);

            foreach (var t in tasks)
                Tasks.Add(t);
        }

        public async Task AddTask(string name)
        {
            TaskModel task = new TaskModel() { Name = name, Checked = false };
            var taskJson = JsonConvert.SerializeObject(task);
            var url = "https://localhost:5001/api/User/" + tripID + "/addTask";

            var result = await _client.PostAsync(url, new StringContent(taskJson, Encoding.UTF8, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                await GetAllTasks(tripID);
            }
        }

        /*public async Task ChangeItem(TaskModel t, int tripID)
        {
            var taskJson = JsonConvert.SerializeObject(t);
            var url =  ;//httpput 

            var res = await _client.PutAsync(url, new StringContent(taskJson, Encoding.UTF8, "application/json"));
        }*/

        public async Task DeleteTaskAsync(int taskID)
        {
            var url = "https://localhost:5001/api/User/" + tripID + "/Task/" + taskID;

            var res = await _client.DeleteAsync(url);

        }
    }
}
