using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWP_Assignment.Services
{
    class CheckValid
    {
        private bool _isAllValid;
        private bool _isEmailValid;
        private bool _isPasswordValid;
        private bool _isFirstNameValid;
        private bool _isLastNameValid;
        private bool _isPhoneValid;
        private bool _isAddressValid;

        public bool IsAllValid { get => _isAllValid; set => _isAllValid = value; }
        public bool IsEmailValid { get => _isEmailValid; set => _isEmailValid = value; }
        public bool IsPasswordValid { get => _isPasswordValid; set => _isPasswordValid = value; }
        public bool IsFirstNameValid { get => _isFirstNameValid; set => _isFirstNameValid = value; }
        public bool IsLastNameValid { get => _isLastNameValid; set => _isLastNameValid = value; }
        public bool IsPhoneValid { get => _isPhoneValid; set => _isPhoneValid = value; }
        public bool IsAddressValid { get => _isAddressValid; set => _isAddressValid = value; }
        

        public bool CheckEmail(TextBox Email, TextBlock Email_Message, SymbolIcon Email_Symbol)
        {
            Email_Symbol.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (Email.Text == "" || Email.Text == null)
            {
                Email_Message.Text = "Please enter your email address";
                Email.BorderBrush = new SolidColorBrush(Colors.Red);
                Email_Symbol.Symbol = Symbol.Cancel;
                Email_Symbol.Foreground = new SolidColorBrush(Colors.Red);
                IsEmailValid = false;
            }
            else
            {
                Email_Message.Text = "";
                Email.BorderBrush = new SolidColorBrush(Colors.Green);
                Email_Symbol.Symbol = Symbol.Accept;
                Email_Symbol.Foreground = new SolidColorBrush(Colors.Green);
                IsEmailValid = true;
            }
            return IsEmailValid;
        }

        public bool CheckPassword(PasswordBox Password, TextBlock Password_Message, SymbolIcon Password_Symbol)
        {
            Password_Symbol.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (Password.Password == "" || Password.Password == null)
            {
                Password_Message.Text = "Please enter your password";
                Password.BorderBrush = new SolidColorBrush(Colors.Red);
                Password_Symbol.Symbol = Symbol.Cancel;
                Password_Symbol.Foreground = new SolidColorBrush(Colors.Red);
                IsPasswordValid = false;
            }
            else
            {
                Password_Message.Text = "";
                Password.BorderBrush = new SolidColorBrush(Colors.Green);
                Password_Symbol.Symbol = Symbol.Accept;
                Password_Symbol.Foreground = new SolidColorBrush(Colors.Green);
                IsPasswordValid = true;
            }
            return IsPasswordValid;
        }

        public bool CheckFirstName(TextBox FirstName, TextBlock FirstName_Message, SymbolIcon FirstName_Symbol)
        {
            FirstName_Symbol.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (FirstName.Text == "" || FirstName.Text == null)
            {
                FirstName_Message.Text = "Please enter your first name";
                FirstName.BorderBrush = new SolidColorBrush(Colors.Red);                
                FirstName_Symbol.Symbol = Symbol.Cancel;
                FirstName_Symbol.Foreground = new SolidColorBrush(Colors.Red);
                IsFirstNameValid = false;
            }
            else
            {
                FirstName_Message.Text = "";
                FirstName.BorderBrush = new SolidColorBrush(Colors.Green);
                FirstName_Symbol.Symbol = Symbol.Accept;
                FirstName_Symbol.Foreground = new SolidColorBrush(Colors.Green);
                IsFirstNameValid = true;
            }
            return IsFirstNameValid;
        }

        public bool CheckLastName(TextBox LastName, TextBlock LastName_Message, SymbolIcon LastName_Symbol)
        {
            LastName_Symbol.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (LastName.Text == "" || LastName.Text == null)
            {
                LastName_Message.Text = "Please enter your first name";
                LastName.BorderBrush = new SolidColorBrush(Colors.Red);
                LastName_Symbol.Symbol = Symbol.Cancel;
                LastName_Symbol.Foreground = new SolidColorBrush(Colors.Red);
                IsLastNameValid = false;
            }
            else
            {
                LastName_Message.Text = "";
                LastName.BorderBrush = new SolidColorBrush(Colors.Green);                
                LastName_Symbol.Symbol = Symbol.Accept;
                LastName_Symbol.Foreground = new SolidColorBrush(Colors.Green);
                IsLastNameValid = true;
            }
            return IsLastNameValid;
        }

        public bool CheckPhone(TextBox Phone, TextBlock Phone_Message, SymbolIcon Phone_Symbol)
        {
            Phone_Symbol.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (Phone.Text == "" || Phone.Text == null)
            {
                Phone_Message.Text = "Please enter your first name";
                Phone.BorderBrush = new SolidColorBrush(Colors.Red);                
                Phone_Symbol.Symbol = Symbol.Cancel;
                Phone_Symbol.Foreground = new SolidColorBrush(Colors.Red);
                IsPhoneValid = false;
            }
            else
            {
                Phone_Message.Text = "";
                Phone.BorderBrush = new SolidColorBrush(Colors.Green);                
                Phone_Symbol.Symbol = Symbol.Accept;
                Phone_Symbol.Foreground = new SolidColorBrush(Colors.Green);
                IsPhoneValid = true;
            }
            return IsPhoneValid;
        }

        public bool CheckAddress(TextBox Address, TextBlock Address_Message, SymbolIcon Address_Symbol)
        {
            Address_Symbol.Visibility = Windows.UI.Xaml.Visibility.Visible;
            if (Address.Text == "" || Address.Text == null)
            {
                Address_Message.Text = "Please enter your first name";
                Address.BorderBrush = new SolidColorBrush(Colors.Red);
                Address_Symbol.Symbol = Symbol.Cancel;
                Address_Symbol.Foreground = new SolidColorBrush(Colors.Red);
                IsAddressValid = false;
            }
            else
            {
                Address_Message.Text = "";
                Address.BorderBrush = new SolidColorBrush(Colors.Green);
                Address_Symbol.Symbol = Symbol.Accept;
                Address_Symbol.Foreground = new SolidColorBrush(Colors.Green);
                IsAddressValid = true;
            }
            return IsAddressValid;
        }
    }
}
