using System.Windows;
using Hotel.Security;

namespace Hotel;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        StartUpPage sp=new StartUpPage();
        Session.CurrentFrame = MainPage;
        Session.CurrentFrame.Navigate(sp);
    }
}

    