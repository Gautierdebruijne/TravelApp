using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using TravelApp_G15.ViewModels;
using TravelApp_G15.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TravelApp_G15
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        private LoginViewModel loginvm;

        public Login()
        {
            this.InitializeComponent();
            loginvm = new LoginViewModel();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            ValidateEmail();
            ValidateNull();

            LoginUser();
        }

        private void ValidateEmail()
        {
            if (Regex.IsMatch(txtEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))
            {
                txtError.Text = "";
            }
            else
            {
                txtError.Text = "The given email is incorrect!";
            }
        }

        private void ValidateNull()
        {
            if (txtEmail.Text.Equals("") || txtEmail.Text == null)
            {
                txtError.Text = "Email address is required!";
            }
            else if(txtPassword.Password.Equals("") || txtPassword.Password == null)
            {
                txtError.Text = "Password is required!";
            }
        }

        private async void LoginUser()
        {
            try
            {
                await loginvm.Login(txtEmail.Text, txtPassword.Password);

                if (loginvm.Success)
                {
                    this.Frame.Navigate(typeof(Dashboard));
                }
                else
                {
                    txtError.Text = "The given password is incorrect!";
                }
            }
            catch(Exception e)
            {
                txtError.Text = e.Message.ToString();
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Register));
        }
    }
}
