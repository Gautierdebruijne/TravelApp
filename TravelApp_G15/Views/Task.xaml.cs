using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TravelApp_G15.Models;
using TravelApp_G15.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TravelApp_G15.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Task : Page
    {
        private ICollection<TaskModel> tasks = new List<TaskModel>();
        private TaskViewModel taskViewModel;
        public double PercentProgress { get; set; }
        public Task()
        {
            this.InitializeComponent();
            PercentProgress = 100;
            taskViewModel = new TaskViewModel();
            GetAllTasks(taskViewModel);
            ProgressbarPercentageToDo();
        }

        private async void GetAllTasks(TaskViewModel viewModel)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());
            await viewModel.GetAllTasks(tripID);

            tasks = viewModel.Tasks;
            TaskList.ItemsSource = tasks;
            ProgressbarPercentageToDo();
        }

        #region Navigation
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("vacations", typeof(Dashboard)),
            ("items", typeof(TripDetail)),
            ("tasks", typeof(Views.Task))
        };

        private void Navigation_PaneOpened(NavigationView sender, object args)
        {
            Menu.Content = "MENU";
        }

        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                this.Frame.Navigate(typeof(Login));
            }
            else if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavView_Navigate(string navItemTag, Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
        {
            Type _page = null;
            if (navItemTag == "settings")
            {
                _page = typeof(Login);
            }
            else
            {
                var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
                _page = item.Page;
            }
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            var preNavPageType = this.Frame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Type.Equals(preNavPageType, _page))
            {
                this.Frame.Navigate(_page, null, transitionInfo);
            }
        }
        private void Navigation_Loaded(object sender, RoutedEventArgs e)
        {
            var settings = (NavigationViewItem)Navigation.SettingsItem;
            settings.Content = "Logout";
            settings.Icon = new SymbolIcon((Symbol)0xE106);
            settings.FontSize = 20;
        }
        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();
        }
        #endregion

        private async void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int checkedTaskID = (int)(sender as CheckBox).Tag;
            bool isCheckedTask = (bool)(sender as CheckBox).IsChecked;
            string name = (string)(sender as CheckBox).Content;
            TaskModel checkedTask = new TaskModel() { TaskID = checkedTaskID, Name = name, IsCheck = isCheckedTask };

            await taskViewModel.ChangeTask(checkedTask);
            

            /* ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
             //local.Values["taskID"] = checkedTaskID;
             int tripID = Int32.Parse(local.Values["tripID"].ToString());
             // int taskID = Int32.Parse(local.Values["taskID"].ToString());*/

            // taskViewModel.CheckTask(tripID, checkedTaskID);
            ProgressbarPercentageToDo();
        }

        private void ProgressbarPercentageToDo()
        {
            if (tasks.Count != 0)
            {
                double todo = 0;
                foreach (var task in tasks)
                {
                    if(task.IsCheck == false)
                    {
                        todo += 1;
                    }
                    else
                    {
                        todo += 0;
                    }
                }
           
                PercentProgress = (todo / tasks.Count) * 100;
                PercentProgress = Math.Round(PercentProgress, 2);
                ProgressBar.Value = PercentProgress;

                if (PercentProgress != 0)
                {
                    ProgressBarMessage.Text = PercentProgress + "% left to do!";
                }
                else
                {
                    ProgressBarMessage.Text = "You have completed all your tasks!";
                }
               
            }
        }


    }
}
