using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
        private ObservableCollection<Item> items;
        private ObservableCollection<Category> categories;
        private ItemViewModel itemViewModel;
        private CategoryViewModel catViewModel;
        public double PercentProgress { get; set; }

        public TripDetail()
        {
            this.InitializeComponent();

            items = new ObservableCollection<Item>();
            categories = new ObservableCollection<Category>();
            itemViewModel = new ItemViewModel();
            catViewModel = new CategoryViewModel();
            PercentProgress = 100;

            GetAllItems(itemViewModel);
            GetAllCategories(catViewModel);
            ProgressbarPercentageToDo();

        }

        private async void GetAllItems(ItemViewModel viewModel)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());

           // int categoryID = Int32.Parse(local.Values["categoryID"].ToString());
            await viewModel.GetAllItems(tripID);
           // await viewModel.GetItemsByCategorie(tripID, categoryID);
            items = viewModel.Items;
            ItemList.ItemsSource = items;

            ProgressbarPercentageToDo();

        }

        private async void GetAllCategories(CategoryViewModel viewModel)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());
            await viewModel.GetAllCategories(tripID);

            categories = viewModel.Categories;
            CatList.ItemsSource = categories;

            if(categories.Count == 0 || categories == null) {
                ProgressBar.Visibility = Visibility.Collapsed;
            }

            ProgressbarPercentageToDo();

        }

        private async void GetItemsPerCategorie(ItemViewModel viewModel)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());
            int categoryID = Int32.Parse(local.Values["categoryID"].ToString());
            Debug.WriteLine("tripid is" + tripID);
            Debug.WriteLine("catID is" + categoryID);

            try
            {
                await viewModel.GetItemsByCategorie(tripID, categoryID);
            }
            catch(Exception e)
            {
                txtError.Text = e.Message.ToString();
            }

            items = viewModel.CategoryItems;
            ItemList.ItemsSource = items;

            ProgressbarPercentageToDo();
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

        private void btnCategory_Click(object sender, RoutedEventArgs e)
        {
            var categoryID = (sender as Button).Tag;
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            local.Values["categoryID"] = categoryID;

            GetItemsPerCategorie(itemViewModel);
            backButtonCat.Visibility = Visibility.Visible;

            ProgressbarPercentageToDo();
        }

        private void btnBackCategoryItems_Click(object sender, RoutedEventArgs e)
        {
            GetAllItems(itemViewModel);
            backButtonCat.Visibility = Visibility.Collapsed;
            
            ProgressbarPercentageToDo();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            popAddItem.IsOpen = true;

            ProgressbarPercentageToDo();
        }

        private async void popAddItem_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = "";
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());

            //moet in xaml id van item ergens kunnen zetten dat methode additemtocategory eraan kan 
           
            //int cateID = Int32.Parse(local.Values["catID"].ToString());
           //int itemID = (int)(sender as ).Tag;

            if (txtNameItem.Text != "" && txtNameItem.Text != null)
            {
                if(txtAmount.Text != "" && txtAmount.Text != null && Regex.IsMatch(txtAmount.Text, @"^[0-9]"))
                {
                    await itemViewModel.AddItem(tripID, txtNameItem.Text, Int32.Parse(txtAmount.Text));
                    
                    //await itemViewModel.AddItemToCategory(tripID, cateID, itemID);
                    popAddItem.IsOpen = false;
                }
                else
                {
                    txtAmount.Text = "1";

                    await itemViewModel.AddItem(tripID, txtNameItem.Text, Int32.Parse(txtAmount.Text));
                    popAddItem.IsOpen = false;
                }
            }
            else
            {
                txtError.Text = "Name is required!";
            }
            ProgressbarPercentageToDo();

        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());

            ContentDialog deleteItem = new ContentDialog
            {
                Title = "Are you sure you want to delete this trip?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteItem.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                int itemID = Int32.Parse((sender as Button).Tag.ToString());
                await itemViewModel.DeleteItem(tripID, itemID);
                GetAllItems(itemViewModel);
            }
        }

        private async void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int checkedItem = (int)(sender as CheckBox).Tag;
            bool isChecked = (bool)(sender as CheckBox).IsChecked;
            string name = (string)(sender as CheckBox).Content;
          

            Item checkItem = new Item() { ItemID = checkedItem, Name = name, Checked = isChecked};

            await itemViewModel.ChangeItem(checkItem);

            ProgressbarPercentageToDo();
        }

        private void ProgressbarPercentageToDo()
        {
            if (items.Count != 0)
            {
                double todo = 0;
                foreach (var item in items)
                {
                    if (item.Checked == false)
                    {
                        todo += 1;
                    }
                    else
                    {
                        todo += 0;
                    }
                }

                PercentProgress = (todo / items.Count) * 100;
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

        private async void btnPopAddCategory_Click(object sender, RoutedEventArgs e)
        {
            txtErrorCategory.Text = "";
            ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
            int tripID = Int32.Parse(local.Values["tripID"].ToString());

            if (txtNameCategory.Text != "" && txtNameCategory.Text != null)
            {
                await catViewModel.AddCategory(tripID, txtNameCategory.Text);
                popAddCategory.IsOpen = false;
            }
            else
            {
                txtErrorCategory.Text = "Name is required!";
            }
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            popAddCategory.IsOpen = true;
        }
    }
}
