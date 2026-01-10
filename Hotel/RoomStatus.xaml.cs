using System.Windows.Controls;
using Hotel.Admins.RoomStatus.ViewModel;

namespace Hotel.Admins.RoomStatus;


public partial class RoomStatusPage : Page
{
    public RoomStatusPage()
    {
        InitializeComponent();
        var _vm= new RoomStatusPageViewModel();
        DataContext = _vm;
    }
}