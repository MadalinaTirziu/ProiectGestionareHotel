using System.ComponentModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Hotel.Security;
using Hotel.Users.Models;
using Hotel.Customers.Model;
namespace Hotel.SignUp.ViewModel;

public class SignUpPageViewModel : INotifyPropertyChanged
{
    public event EventHandler SignUpSucceeded;
    public event PropertyChangedEventHandler PropertyChanged;
        
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private string _username;
    public string Username
    {
        get { return _username; }
        set { _username = value; OnPropertyChanged(nameof(Username)); }
    }
    private string _password;
    public string Password
    {
        get { return _password; }
        set { _password = value; OnPropertyChanged(nameof(Password)); }
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
    public ICommand SignUpCommand { get; set; }

    public SignUpPageViewModel()
    {
        SignUpCommand = new RelayCommand(Submit);
    }
    private readonly string _jsonPath = "Data.json";
    private void Submit(object parameter)
    {
        try
        {
            if (!File.Exists(_jsonPath))
            {
                MessageBox.Show("Users file not found.");
                return;
            }
            string json=File.ReadAllText(_jsonPath);
            List<User> users = JsonSerializer.Deserialize<List<User>>(json);
            User matchedUser = users.FirstOrDefault(u => u.Username == this.Username);
            if (matchedUser == null)
            {
                CurrentUser = new Customer(Username, Password);
                users.Add(CurrentUser);
                
                string updatedJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_jsonPath, updatedJson);
                
                Session.Login(CurrentUser);
                SignUpSucceeded?.Invoke(this, EventArgs.Empty);
                MessageBox.Show($"User {CurrentUser.Username} was successfully created.");
            }
            else
            {
                MessageBox.Show($"User {matchedUser.Username} already exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
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
