using System.Windows.Controls;
using ExtractToWork.WPF.ViewModels;

namespace ExtractToWork.WPF.Views;

public partial class SelectView : UserControl
{
    public SelectView()
    {
        InitializeComponent();
    }

    private void SettingsButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        App.CurrentViewModel = new ConfigViewModel();
    }
}
