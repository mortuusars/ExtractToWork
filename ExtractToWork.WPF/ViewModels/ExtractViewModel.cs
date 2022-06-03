using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExtractToWork.Core;

namespace ExtractToWork.WPF.ViewModels;

[ObservableObject]
public partial class ExtractViewModel
{
    [ObservableProperty]
    private double _progress;

    [ObservableProperty]
    private string _filesProgress = string.Empty;

    [ObservableProperty]
    private bool _isDone;

    [ObservableProperty]
    private int _filesCount;

    [ObservableProperty]
    private int _extractedCount;

    [ObservableProperty]
    private string _currentFileName = string.Empty;

    private readonly Config _config;

    public ExtractViewModel(Config config)
    {
        _config = config;
    }

    [ICommand]
    private async Task ExtractArchives(string[] archiveFilePaths)
    {
        string destinationPath = _config.DestinationPath;
        if (_config.CreateCurrentDateFolder)
            destinationPath = Path.Combine(destinationPath, DateTime.Now.ToString("yyyy-MM-dd"));

        PathGenerator pathCreator = new(destinationPath, (arcName) => arcName + _config.AppendText);
        NewZipExtractor zipExtractor = new(pathCreator, archiveFilePaths);

        IEnumerable<ZipFileInfo> info = await zipExtractor.GetInfo();
        _filesCount = info.Select(i => i.FileNames.Count()).Sum();

        zipExtractor.ArchiveExtracted += (_, e) =>
        {
            if (_config.CreateEmptyFolder)
            {
                try
                {
                    string archiveName = Path.GetFileNameWithoutExtension(e.FilePath);
                    string archiveWithoutAppend = pathCreator.Create(archiveName, useModifier: false);
                    Directory.CreateDirectory(archiveWithoutAppend);
                }
                catch (Exception) { Console.WriteLine("Irrelevant."); }
            }
        };
        zipExtractor.CurrentlyExtractedFile += ZipExtractor_CurrentlyExtractedFile;
        zipExtractor.FileExtracted += ReportProgress;

        FinishedExtraction finishedEx = await zipExtractor.Extract();
        FilesProgress = $"DONE! Extracted {finishedEx.TotalFiles} files";
        IsDone = true;

        System.Media.SystemSounds.Beep.Play();

        InformIfHasErrored(finishedEx.FailedFiles); 


        if (_config.OpenDestinationFolderWhenCompleted)
            Utils.OpenFolderInExplorer(destinationPath);

        if (_config.CloseWhenCompleted)
            App.CloseAfter(2);
    }

    private void ZipExtractor_CurrentlyExtractedFile(object? sender, string e)
    {
        CurrentFileName = Path.GetFileName(e);
    }

    private void InformIfHasErrored(IEnumerable<FailedFile> failedFiles)
    {
        int count = failedFiles.Count();
        if (count > 0 && count < 30)
        {
            string message = "Some files were not extracted:\n\n" + string.Join('\n', failedFiles);
            MessageBox.Show(message, App.AppName, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        else if (count >= 30)
            MessageBox.Show(count + " files were not extracted.");
    }

    private void ReportProgress(object? sender, ExtractInfo e)
    {
        ExtractedCount = e.ExtractedCount;
        Progress = ((double)ExtractedCount / (double)FilesCount) * 100d;
        FilesProgress = $"{ExtractedCount} of {FilesCount}";
    }
}
