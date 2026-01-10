using System.Windows;
using System.Windows.Controls;
using Hotel.Admins.EditReservationStatus.ViewModel;
using Hotel.Reservation.Model;
using Hotel.Security;

namespace Hotel.Admins.EditReservationStatus;

public partial class AdminEditReservationStatusPage : Page
{
    public AdminEditReservationStatusPage(Rezervare rezervare)
    {
        InitializeComponent();
        var _vm = new AdminEditReservationStatusViewModel(rezervare);
        DataContext = _vm;
        GoBackButton.Click += ExecuteGoBack;
    }

    public void ExecuteGoBack(object sender, RoutedEventArgs e)
    {
        Session.CurrentFrame.GoBack();
    }
}