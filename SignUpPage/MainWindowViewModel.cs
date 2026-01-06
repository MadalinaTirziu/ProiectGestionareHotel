
using System.ComponentModel;
using Hotel.Users.Models;
namespace Hotel.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    bool IsConnected { get; set; }

    public MainWindowViewModel()
    {
        IsConnected = false;
    }
}