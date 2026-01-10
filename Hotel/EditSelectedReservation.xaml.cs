using System.Windows.Controls;
using Hotel.EditSelectedReservation.ViewModel;
using Hotel.Reservation.Model;

namespace Hotel.EditSelectedReservation;

public partial class EditSelectedReservationPage : Page
{
    public EditSelectedReservationPage(Rezervare rezervare)
    {
        InitializeComponent();
        var _vm = new EditSelectedReservationPageViewModel(rezervare);
        DataContext = _vm;
    }
}