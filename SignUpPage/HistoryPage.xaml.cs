using System.Windows.Controls;
using Hotel.History.ViewModel;

namespace Hotel.History;

public partial class HistoryPage : Page
{
    private readonly HistoryPageViewModel _vm;

    public HistoryPage()
    {
        InitializeComponent();
        _vm = new HistoryPageViewModel();
        this.DataContext = _vm; // This links your UI to your logic
    }
}