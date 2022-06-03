using System.Windows;
using System.Windows.Threading;
using ExtractToWork.Core;
using ExtractToWork.WPF.ViewModels;

namespace ExtractToWork.WPF;

public partial class App : Application
{
    public static string AppName { get; } = "ExtractToWork";
    public static Config Config { get; } = new Config();

    public static object CurrentViewModel { get => _mainViewModel.CurrentViewModel; set => _mainViewModel.CurrentViewModel = value; }

    private MainWindow? _mainWindow;
    private static MainWindowViewModel _mainViewModel = new(Config);

    protected override void OnStartup(StartupEventArgs e)
    {
        Config.Load();

        _mainWindow = new MainWindow();

        if (new ArgumentProcessor().IsAllArgsAreFilePaths(e.Args))
        {
            ExtractViewModel extractVM = new ExtractViewModel(Config);
            _mainViewModel.CurrentViewModel = extractVM;
            extractVM.ExtractArchivesCommand.Execute(e.Args);
        }
        else
            _mainViewModel.CurrentViewModel = new SelectViewModel();

        _mainWindow.DataContext = _mainViewModel;
        _mainWindow.Show();
    }

    internal static void CloseAfter(double seconds)
    {
        var timer = new DispatcherTimer(TimeSpan.FromSeconds(seconds), DispatcherPriority.Normal, OnCloseTimerFinished, App.Current.Dispatcher);
        timer.Start();
    }

    private static void OnCloseTimerFinished(object? sender, EventArgs e)
    {
        if (sender is DispatcherTimer dt)
            dt.Stop();

        Environment.Exit(0);
    }
}