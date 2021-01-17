using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class LocationPage : Page
    {

        private ObservableCollection<Location> locations;
        private LocationViewModel locationViewModel;

        public LocationPage()
        {
            this.InitializeComponent();

            locations = new ObservableCollection<Location>();
            locationViewModel = new LocationViewModel();

            GetAllLocations(locationViewModel);
        }

        private async void GetAllLocations(LocationViewModel viewModel)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());

            await viewModel.GetLocations(tripID);

            locations = viewModel.Locations;
            LocList.ItemsSource = locations;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            popAdd.IsOpen = true;
        }

        private async void btnAddLocation_ClickAsync(object sender, RoutedEventArgs e)
        {
            txtError.Text = "";
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());

            if (txtCountry.Text != "" && txtCountry.Text != null)
            {
                if (txtCity.Text != "" && txtCity != null)
                {
                    await locationViewModel.AddLocation(tripID, txtCountry.Text, txtCity.Text);
                    popAdd.IsOpen = false;
                }
            }
            else
            {
                txtError.Text = "Fill in all fields";
            }
        }

        #region Navigation
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("vacations", typeof(Dashboard)),
            ("items", typeof(TripDetail)),
            ("tasks", typeof(Views.Task)),
            ("locations", typeof(Views.LocationPage))
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
            settings.FontSize = 20;
            settings.Icon = new SymbolIcon((Symbol)0xE106);
        }
        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();
        }
        #endregion

    }
}
