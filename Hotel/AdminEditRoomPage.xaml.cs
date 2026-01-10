using System.Windows.Controls;
using Hotel.Admins.EditRoom.ViewModel;
using Hotel.Room.Model;

namespace Hotel.Admins.EditRoom;

public partial class AdminEditRoomPage : Page
{
    public AdminEditRoomPage(Camera selectedRoom)
    {
        InitializeComponent();
        var _vm = new AdminEditRoomPageViewModel(selectedRoom);
        DataContext = _vm;
    }
}