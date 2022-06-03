using System.Windows.Controls;
using ExtractToWork.WPF.ViewModels;

namespace ExtractToWork.WPF.Views;
public partial class ConfigView : UserControl
{
    public ConfigView()
    {
        InitializeComponent();
    }

    private void SettingsBackButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        App.CurrentViewModel = new SelectViewModel();
    }
}
