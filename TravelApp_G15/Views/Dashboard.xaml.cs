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
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TravelApp_G15.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Dashboard : Page
    {
        private ICollection<Trip> _trips;
        private TripViewModel tripViewModel;

        public Dashboard()
        {
            this.InitializeComponent();
            tripViewModel = new TripViewModel();
            _trips = new List<Trip>();

            GetAllTrips(tripViewModel);
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
                }
            }
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            var settings = (NavigationViewItem)Navigation.SettingsItem;
            settings.Content = "Logout";
            settings.Icon = new SymbolIcon((Symbol)0xE106);
        }

        private void Navigation_PaneOpened(NavigationView sender, object args)
        {
            Menu.Content = "Menu";
        }

        private async void GetAllTrips(TripViewModel viewModel)
        {
            await viewModel.GetAllTrips();
            _trips = viewModel.Trips;

            Vacations.ItemsSource = _trips;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tripID = (sender as Button).Tag;

            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            local.Values["tripID"] = tripID;

            this.Frame.Navigate(typeof(TripDetail));
        }
    }
}