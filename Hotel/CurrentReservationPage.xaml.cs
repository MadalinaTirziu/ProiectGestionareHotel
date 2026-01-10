using System.Windows.Controls;
using Hotel.CurrentReservation.ViewModel;
using Hotel.Security;

namespace Hotel.CurrentReservation;

public partial class CurrentReservationPage : Page
{
    public CurrentReservationPage()
    {
        InitializeComponent();
        var _vm = new CurrentReservationPageViewModel();
        this.DataContext = _vm;
        
    }
}