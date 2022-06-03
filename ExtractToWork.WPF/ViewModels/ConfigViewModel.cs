using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace ExtractToWork.WPF.ViewModels;

[ObservableObject]
public partial class ConfigViewModel
{
    public Config Config { get; } = App.Config;

    [ICommand]
    private void SelectFolder()
    {
        var dialog = new VistaFolderBrowserDialog
        {
            ShowNewFolderButton = true,
            Multiselect = false
        };

        if (dialog.ShowDialog() is true)
            Config.DestinationPath = dialog.SelectedPath;
    }
}
