using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ookii.Dialogs.Wpf;

namespace ExtractToWork.WPF.ViewModels;

[ObservableObject]
public partial class SelectViewModel
{
    public ObservableCollection<FileInfo> SelectedArchives { get; } = new();
    
    [ICommand]
    private void SelectArchives()
    {
        var dialog = new VistaOpenFileDialog();
        dialog.Multiselect = true;
        dialog.Filter = "Zip Archives(*.zip)|*.zip; | All files(*.*)|*.*";

        if (dialog.ShowDialog() is true)
        {
            foreach (string path in dialog.FileNames)
                SelectedArchives.Add(new FileInfo(path));
        }
    }

    [ICommand]
    private void Extract()
    {
        var extractVM = new ExtractViewModel(App.Config);
        App.CurrentViewModel = extractVM;
        extractVM.ExtractArchivesCommand.Execute(SelectedArchives.Select(f => f.FullName).ToArray());
    }

    [ICommand]
    private void RemoveSelectedArchive(FileInfo archive)
    {
        SelectedArchives.Remove(archive);
    }
}
