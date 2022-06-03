using CommunityToolkit.Mvvm.ComponentModel;

namespace ExtractToWork.WPF.ViewModels;

[ObservableObject]
public partial class MainWindowViewModel
{
    public Config Config { get; }

    [ObservableProperty]
    private object _currentViewModel;

    public MainWindowViewModel(Config config)
    {
        Config = config;
        _currentViewModel = new SelectViewModel();
    }

    public void Extract(string[] paths)
    {
        ExtractViewModel extractVM = new(Config);
        _currentViewModel = extractVM;

        extractVM.ExtractArchivesCommand.Execute(paths);
    }
}
