using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
        public Login()
        {
            this.InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
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

        private void LoginUser()
        {

        }
    }
}
