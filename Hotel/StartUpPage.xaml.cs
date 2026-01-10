
using Hotel.Users.Models;
using System.Windows;
using System.Windows.Controls;
using Hotel.Admins.ReservationStatus;
using Hotel.Admins.RoomStatus;
using Hotel.CurrentReservation;
using Hotel.SignIn;
using Hotel.Security;
using Hotel.SignUp;
using Hotel.History;
using Hotel.Room.Model;
using Hotel.RoomPage;

namespace Hotel;

public partial class StartUpPage : Page
{
    private static StartUpPageViewModel _vm = new StartUpPageViewModel();

    public StartUpPage()
    {
        InitializeComponent();      
        this.DataContext = _vm;
        SignInButton.Click += SignInButton_Click;
        SignUpButton.Click += SignUpButton_Click;
        HistoryButton.Click += HistoryButton_Click;
        LogOutButton.Click += LogOutButton_Click;
        CurrentReservationButton.Click += CurrentReservationButton_Click;
        RoomStatusButton.Click += RoomStatusButton_Click;
        ReservationStatusButton.Click += ReservationStatusButton_Click;
    }

    private void SignInButton_Click(object sender, RoutedEventArgs e)
    {
        SignInPage signInPage = new SignInPage();
        signInPage.LoginSucceeded += SignInPage_LoginSucceeded;

        Session.CurrentFrame.Navigate(signInPage);
    }

    private void SignUpButton_Click(object sender, RoutedEventArgs e)
    {
        SignUpPage signUpPage = new SignUpPage();
        signUpPage.SignUpSucceeded += SignUpPage_SignUpSucceeded;
        Session.CurrentFrame.Navigate(signUpPage);
    }

    private void HistoryButton_Click(object sender, RoutedEventArgs e)
    {
        HistoryPage historyPage = new HistoryPage();
        Session.CurrentFrame.Navigate(historyPage);
    }

    private void LogOutButton_Click(object sender, RoutedEventArgs e)
    {
        Session.Logout();
        
    }

    private void RoomStatusButton_Click(object sender, RoutedEventArgs e)
    {
        RoomStatusPage roomStatusPage = new RoomStatusPage();
        Session.CurrentFrame.Navigate(roomStatusPage);
    }

    private void ReservationStatusButton_Click(object sender, RoutedEventArgs e)
    {   
        ReservationStatusPage reservationStatusPage = new ReservationStatusPage();
        Session.CurrentFrame.Navigate(reservationStatusPage);
    }
    private void SignInPage_LoginSucceeded(object sender, EventArgs e)
    {
        
        Session.CurrentFrame.GoBack();
    }
    

    private void SignUpPage_SignUpSucceeded(object sender, EventArgs e)
    {
        
        Session.CurrentFrame.GoBack();
    }

    private void CameraButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var selectedCamera = button.DataContext as Camera;
        if (selectedCamera != null)
        {
            CameraPage cameraPage = new CameraPage(selectedCamera);
            Session.CurrentFrame.Navigate(cameraPage);
        }
    }

    private void CurrentReservationButton_Click(object sender, RoutedEventArgs e)
    {
        CurrentReservationPage currentReservationPage = new CurrentReservationPage();
        Session.CurrentFrame.Navigate(currentReservationPage);
    }
    
}