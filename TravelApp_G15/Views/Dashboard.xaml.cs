﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TravelApp_G15.Models;
using TravelApp_G15.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
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
        private ObservableCollection<Trip> _trips;
        private TripViewModel tripViewModel;
        private LocationViewModel locationViewModel;

        public Dashboard()
        {
            this.InitializeComponent();
            tripViewModel = new TripViewModel();
            locationViewModel = new LocationViewModel();
            _trips = new ObservableCollection<Trip>();

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
            settings.FontSize = 20;
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

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (this.Frame.CanGoBack) this.Frame.GoBack();
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var tripID = (sender as Button).Tag;

            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            local.Values["tripID"] = tripID;

            this.Frame.Navigate(typeof(TripDetail));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            popAdd.IsOpen = true;
        }

        private async void btnAddTrip_Click(object sender, RoutedEventArgs e)
        {
            String date = "";
            DateTime departure = DateTime.Now;

            if (txtName.Text != "" && txtName.Text != null)
            {
                if(txtCountry.Text != "" && txtCountry.Text != null)
                {
                    if(txtCity.Text != "" && txtCity.Text != null)
                    {
                        try
                        {
                            date = datePicker.Date.Value.ToString();
                            departure = Convert.ToDateTime(date);

                            if (departure < DateTime.Now)
                            {
                                txtError.Text = "Departure date can't be in the past!";
                            }
                            else
                            {
                                await tripViewModel.AddTrip(txtName.Text, txtCountry.Text, txtCity.Text, departure);
                                //AddLocation(txtCountry.Text, txtCity.Text);

                                popAdd.IsOpen = false;
                            }
                        }
                        catch
                        {
                            txtError.Text = "Departure date is required!";
                        }
                    }
                    else
                    {
                        txtError.Text = "City is required!";
                    }
                }
                else
                {
                    txtError.Text = "Country is required!";
                }
            }
            else
            {
                txtError.Text = "Name is required!";
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog deleteTrip = new ContentDialog
            {
                Title = "Are you sure you want to delete this trip?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteTrip.ShowAsync();

            if(result == ContentDialogResult.Primary)
            {
                int tripID = Int32.Parse((sender as Button).Tag.ToString());
                await tripViewModel.DeleteTrip(tripID);
            }
        }

        //private async void AddLocation(string country, string city)
        //{
        //    ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
        //    int tripID = Int32.Parse(local.Values["tripID"].ToString());

        //    await locationViewModel.AddLocation(tripID, country, city);
        //}
    }
}