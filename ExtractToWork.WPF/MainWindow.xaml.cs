using System.Windows;
using ExtractToWork.WPF.ViewModels;

namespace ExtractToWork.WPF;

public partial class MainWindow : Window
{
    public bool IsDragOver
    {
        get { return (bool)GetValue(IsDragOverProperty); }
        set { SetValue(IsDragOverProperty, value); }
    }

    public static readonly DependencyProperty IsDragOverProperty =
        DependencyProperty.Register(nameof(IsDragOver), typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

    public MainWindow()
    {
        InitializeComponent();
        this.MouseDown += (_, _) => this.DragMove();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => App.Current.Shutdown();

    private void window_PreviewDragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            IsDragOver = true;
    }

    private void window_PreviewDrop(object sender, DragEventArgs e)
    {
        IsDragOver = false;
    }

    private void window_PreviewDragLeave(object sender, DragEventArgs e)
    {
        IsDragOver = false;
    }

    private void DashBorder_Drop(object sender, DragEventArgs e)
    {
        this.DataContext.CastTo<MainWindowViewModel>();
    }
}
