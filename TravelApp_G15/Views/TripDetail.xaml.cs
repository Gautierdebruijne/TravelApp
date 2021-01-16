using System;
using System.Collections.Generic;
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
    public sealed partial class TripDetail : Page
    {
        private ICollection<Item> items;
        private ICollection<TaskModel> tasks;
        private ICollection<Category> categories;
        private ItemViewModel itemViewModel;
        private TaskViewModel taskViewModel;
        private CategoryViewModel catViewModel;

        public TripDetail()
        {
            this.InitializeComponent();

            items = new List<Item>();
            categories = new List<Category>();
            itemViewModel = new ItemViewModel();
            taskViewModel = new TaskViewModel();
            catViewModel = new CategoryViewModel();

            GetAllItems(itemViewModel);
            GetAllTasks(taskViewModel);
            GetAllCategories(catViewModel);
        }

        private void Navigation_PaneOpened(NavigationView sender, object args)
        {
            Menu.Content = "Menu";
        }

        private void Navigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                this.Frame.Navigate(typeof(Login));
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                switch (item.Tag.ToString())
                {
                    case "Vacations": this.Frame.Navigate(typeof(Dashboard)); break;
                    case "Items": this.Frame.Navigate(typeof(TripDetail)); break;
                    case "Tasks": this.Frame.Navigate(typeof(TripDetail)); break;
                    case "Categories": this.Frame.Navigate(typeof(TripDetail)); break;
                }
            }
        }

        private void Navigation_Loaded(object sender, RoutedEventArgs e)
        {
            var settings = (NavigationViewItem)Navigation.SettingsItem;
            settings.Content = "Logout";
            settings.Icon = new SymbolIcon((Symbol)0xE106);
        }

        private async void GetAllItems(ItemViewModel viewModel)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());
            await viewModel.GetAllItems(tripID);

            items = viewModel.Items;
            ItemList.ItemsSource = items;
        }

        private async void GetAllTasks(TaskViewModel viewModel)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());
            await viewModel.GetAllTasks(tripID);

            tasks = viewModel.Tasks;
            TaskList.ItemsSource = tasks;
        }

        private async void GetAllCategories(CategoryViewModel viewModel)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());
            await viewModel.GetAllCategories(tripID);

            categories = viewModel.Categories;
            CatList.ItemsSource = categories;
        }
        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();
        }
    }
}
