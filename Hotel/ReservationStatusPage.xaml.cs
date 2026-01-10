using System.Windows;
using System.Windows.Controls;
using Hotel.Admins.ReservationStatus.ViewModel;
using Hotel.Security;

namespace Hotel.Admins.ReservationStatus;

public partial class ReservationStatusPage : Page
{
    public ReservationStatusPage()
    {
        InitializeComponent();
        var _vm = new ReservationStatusViewModel();
        DataContext = _vm;
        GoBackButton.Click += GoBack;
    }

    public void GoBack(object sender, RoutedEventArgs e)
    {
        Session.CurrentFrame.GoBack();
    }
}