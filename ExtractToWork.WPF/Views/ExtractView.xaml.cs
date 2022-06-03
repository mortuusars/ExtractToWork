using System.Windows;
using System.Windows.Controls;

namespace ExtractToWork.WPF.Views;

public partial class ExtractView : UserControl
{
    public ExtractView()
    {
        InitializeComponent();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}
