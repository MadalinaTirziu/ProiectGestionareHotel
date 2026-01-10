using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Hotel.Users.Models;

using Hotel.ViewModel;
using Hotel;
using Hotel.Security;

namespace Hotel.SignIn.ViewModel;

    public class SignInPageViewModel : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        
        public ICommand SubmitCommand { get; }

        
        public SignInPageViewModel()
        {
            SubmitCommand = new RelayCommand(Submit); 
        }

        public event EventHandler LoginSucceeded;
        private readonly string _jsonPath = "users.json";
        private void Submit(object parameter)
        {
            try
            {
                
                if (!File.Exists(_jsonPath))
                {
                    MessageBox.Show("Users file not found.");
                    return;
                }

                string json = File.ReadAllText(_jsonPath);

                
                List<User> users = JsonSerializer.Deserialize<List<User>>(json);

                
                User matchedUser= users.Find(u =>
                    u.Username.Equals(UserName, StringComparison.Ordinal) &&
                    u.Password.Equals(Password, StringComparison.Ordinal));;

                if (matchedUser != null)
                {
                    CurrentUser = matchedUser;
                    MessageBox.Show($"Successfully logged in as {CurrentUser.Username} as {CurrentUser.Role}.");
                    Session.Login(CurrentUser);
                    LoginSucceeded?.Invoke(this, EventArgs.Empty);
                    
                }
                else
                {
                    MessageBox.Show("Incorrect username or password.");
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading users: " + ex.Message);
            }
        }
    }

    
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
