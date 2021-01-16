using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using TravelApp_G15.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Register : Page
    {
        public Register()
        {
            this.InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login));
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            ValidateLogin();
            RegisterUser();
        }

        private void txtPasswordConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePassword();
        }

        private void ValidateLogin()
        {
            if (txtEmail.Text != "" && txtEmail.Text != null && txtName.Text != "" && txtName.Text != null &&
                txtPassword.Password != "" && txtPassword.Password != null && txtPasswordConfirm.Password != "" && txtPasswordConfirm.Password != null)
            {
                txtError.Text = "";

                if (Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    if (Regex.IsMatch(txtPassword.Password, @"^(.{0,7}|[^0-9]*|[^A-Z]*|[^a-z]*|[a-zA-Z0-9]*)$"))
                    {
                        txtError.Text = "A password must have a minimum of eight characters, \nat least one uppercase letter, one lowercase letter, \none number and one special character";
                    }
                    else
                    {
                        if (txtPassword.Password == txtPasswordConfirm.Password)
                            txtError.Text = "";
                        else
                            txtError.Text = "The given passwords do not match!";
                    }
                }
                else
                {
                    txtError.Text = "The given email is incorrect!";
                }
            }
            else
            {
                txtError.Text = "All fields are required!";
            }
        }

        private void ValidatePassword()
        {
            txtError.Text = "";

            if (Regex.IsMatch(txtPassword.Password, @"^(.{0,7}|[^0-9]*|[^A-Z]*|[^a-z]*|[a-zA-Z0-9]*)$"))
            {
                txtError.Text = "A password must have a minimum of seven characters, \nat least one uppercase letter, one lowercase letter, \none number and one special character";
            }
            else
            {
                if (txtPassword.Password == txtPasswordConfirm.Password)
                    txtError.Text = "";
                else
                    txtError.Text = "The given passwords do not match!";
            }
        }

        private async void RegisterUser()
        {
            var registervm = new RegisterViewModel();

            try
            {
                await registervm.Register(txtName.Text, txtEmail.Text, txtPassword.Password);

                if (registervm.Success)
                {
                    Debug.WriteLine("Succesvol geregistreerd");
                    this.Frame.Navigate(typeof(Dashboard));
                }
                else
                {
                    Debug.WriteLine("Onsuccesvol geregistreerd");
                    txtError.Text = "Something has gone wrong, has this email already been used?";
                }
            }
            catch(Exception e)
            {
                txtError.Text = e.Message.ToString();
            }
        }
    }
}
