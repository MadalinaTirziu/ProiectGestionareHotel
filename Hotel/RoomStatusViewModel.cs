using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Hotel.Admins.EditRoom;
using Hotel.Room.Admin;
using Hotel.Room.Model;
using Hotel.Security;

namespace Hotel.Admins.RoomStatus.ViewModel;


public class RoomStatusPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    private AdministrareCamere _ac = new AdministrareCamere();
    public ObservableCollection<Camera> Camere { get; private set; }
    public ICommand EditRoomCommand { get; }
    public ICommand DeleteRoomCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand AddRoomCommand { get; }
    public ICommand ConfirmAddRoomCommand { get; }
    
    public RoomStatusPageViewModel()
    {
        var sortedList = _ac.AfisareCamere().OrderBy(c => c.Numar).ToList();
        Camere = new ObservableCollection<Camera>(sortedList);
        VerificareVizibilitati();
        EditRoomCommand = new RelayCommand(camera => EditRoom(camera));
        DeleteRoomCommand = new RelayCommand(camera => DeleteRoom(camera));
        GoBackCommand = new RelayCommand(_ => ExecuteGoBack(_));
        AddRoomCommand = new RelayCommand(_ => AddRoom(_));
        ConfirmAddRoomCommand = new RelayCommand(_ => AddRoomToList(_));
        Session.SessionChanged += Refresh;
        Refresh();
    }
    
    private Visibility _activeVisibility = Visibility.Visible;
    public Visibility ActiveVisibility
    {
        get => _activeVisibility;
        set
        {
            _activeVisibility = value;
            OnPropertyChanged(nameof(ActiveVisibility));
        }
    }
    private Visibility _inactiveVisibility = Visibility.Collapsed;
    public Visibility InactiveVisibility
    {
        get => _inactiveVisibility;
        set
        {
            _inactiveVisibility = value;
            OnPropertyChanged(nameof(InactiveVisibility));
        }
    }
    private Visibility _addRoomVisibility = Visibility.Collapsed;

    public Visibility AddRoomVisibility
    {
        get => _addRoomVisibility;
        set
        {
            _addRoomVisibility = value;
            OnPropertyChanged(nameof(AddRoomVisibility));
        }
    }

    private void VerificareVizibilitati()
    {
        if (Camere.Count == 0)
        {
            InactiveVisibility = Visibility.Visible;
            ActiveVisibility = Visibility.Collapsed;
        }
        else
        {
            InactiveVisibility = Visibility.Collapsed;
            ActiveVisibility = Visibility.Visible;
        }
    }

    private void EditRoom(object parameter)
    {
        if (parameter is Camera camera)
        {
            var adminEditRoom = new AdminEditRoomPage(camera);
            Session.CurrentFrame.Navigate(adminEditRoom);
        }
    }

    private void DeleteRoom(object parameter)
    {
        if (parameter is Camera room)
        {
            Camere.Remove(room);
            _ac.StergeCamera(room.Numar);
            
            VerificareVizibilitati();
        }
    }

    private void ExecuteGoBack(object parameter)
    {
        Session.CurrentFrame.GoBack();
    }

    private void AddRoom(object parameter)
    {
        AddRoomVisibility = Visibility.Visible;
    }
    public List<StatusCamera> StatusOptions { get; } = Enum.GetValues(typeof(StatusCamera)).Cast<StatusCamera>().ToList();
    private StatusCamera _selectedStatus;
    public StatusCamera SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            _selectedStatus = value;
            OnPropertyChanged(nameof(SelectedStatus));
            
            // You can trigger logic immediately when they select something here:
            //UpdateRoomStatus(value);
        }
    }
    private int? _newRoomNumber;
    public int? NewRoomNumber
    {
        get => _newRoomNumber;
        set
        {
            _newRoomNumber = value;
            OnPropertyChanged(nameof(NewRoomNumber));
        }
    }

    private void AddRoomToList(object parameter)
    {
        _ac.AdaugaCamera(NewRoomNumber.Value,SelectedStatus);
        var sortedList = _ac.AfisareCamere().OrderBy(c => c.Numar).ToList();
        Camere = new ObservableCollection<Camera>(sortedList);
        OnPropertyChanged(nameof(Camere));
    }

    private void Refresh()
    {
        _ac = new AdministrareCamere(); 
        var sortedList = _ac.AfisareCamere().OrderBy(c => c.Numar).ToList();
        Camere = new ObservableCollection<Camera>(sortedList);
        OnPropertyChanged(nameof(Camere));
        VerificareVizibilitati();
    }
}
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
    
    public void Execute(object parameter) => _execute(parameter);

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }
}