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
    class TaskViewModel
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }
        private HttpClient _client;
        private string _apiUrl = "https://travelappg15api.azurewebsites.net/api";
        //private string _apiUrl = "https://localhost:5001/api";

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

            Tasks = new ObservableCollection<TaskModel>();

        }

        public async Task GetAllTasks(int tripID)
        {
            var url = _apiUrl + "/User/" + tripID + "/tasks";
            var json = await _client.GetStringAsync(url);
            var tasks = JsonConvert.DeserializeObject<ICollection<TaskModel>>(json);

            Tasks.Clear();

            foreach (var t in tasks)
                Tasks.Add(t);
        }

        public async Task AddTask(string name, int tripID)
        {
            TaskModel task = new TaskModel() { Name = name, IsCheck = false };
            var taskJson = JsonConvert.SerializeObject(task);
            var url = _apiUrl + "/User/" + tripID + "/addTask";

            var result = await _client.PostAsync(url, new StringContent(taskJson, Encoding.UTF8, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                await GetAllTasks(tripID);
            }
        }

        public async Task ChangeItem(int taskID, int tripID)
        {

            var url = _apiUrl + "/User/" + tripID + "/Item/" + taskID;

            var res = await _client.PutAsync(url, null);
           }
   
        public async Task ChangeTask(TaskModel task)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());

            var taskJson = JsonConvert.SerializeObject(task);
            var url = "https://localhost:5001/api/User/" + tripID + "/Tasks/" + task.TaskID;

            var res = await _client.PutAsync(url, new StringContent(taskJson, Encoding.UTF8, "application/json"));
        }

        public async Task DeleteTaskAsync(int tripID, int taskID)
        {
            var url = _apiUrl + "/User/" + tripID + "/Task/" + taskID;

            var res = await _client.DeleteAsync(url);

        }
    }
}
