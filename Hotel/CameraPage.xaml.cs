using System.Windows.Controls;
using Hotel.Room.Model;
using Hotel.RoomPage.ViewModel;

namespace Hotel.RoomPage;

public partial class CameraPage : Page
{
    public CameraPage(Camera room)
    {
        InitializeComponent();
        var _vm = new CameraPageViewModel(room);
        this.DataContext = _vm;
    }
}