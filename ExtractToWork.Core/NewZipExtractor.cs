using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractToWork.Core;

public record ZipFileInfo(string FilePath, IEnumerable<string> FileNames);

public record ExtractInfo(int ExtractedCount, string LastExtractedFile);

public record FailedFile(string FilePath, Exception Exception);

public record FinishedExtraction(int TotalFiles, IEnumerable<FailedFile> FailedFiles);

public class NewZipExtractor
{
    public event EventHandler<ExtractInfo>? FileExtracted;
    public event EventHandler<string>? CurrentlyExtractedFile;
    public event EventHandler<ZipFileInfo>? ArchiveExtracted;

    private readonly IPathGenerator pathCreator;
    private readonly IEnumerable<string> _archiveFilePaths;

    private int _filesCount;

    public NewZipExtractor(IPathGenerator pathCreator, IEnumerable<string> archiveFilePaths)
    {
        foreach (var path in archiveFilePaths)
            if (!File.Exists(path))
                throw new InvalidOperationException("File not exists: " + path);

        this.pathCreator = pathCreator;
        _archiveFilePaths = archiveFilePaths;
    }

    public async Task<IEnumerable<ZipFileInfo>> GetInfo()
    {
        List<ZipFileInfo> infos = new();

        foreach (string path in _archiveFilePaths)
        {
            using (var archive = await Task.Run(() => ZipFile.OpenRead(path)))
            {
                int filesCount = archive.Entries.Select(e => e.Name.Length > 0).Count();
                infos.Add(new ZipFileInfo(path, archive.Entries.Select(e => e.Name)));
            }
        }

        return infos;
    }

    public async Task<FinishedExtraction> Extract()
    {
        List<FailedFile> failedFiles = new();
        int extractedCount = 0;

        foreach (string path in _archiveFilePaths)
        {
            using (var archive = await Task.Run(() => ZipFile.OpenRead(path)))
            {
                string destinationPath = pathCreator.Create(Path.GetFileNameWithoutExtension(path));
                Directory.CreateDirectory(destinationPath);
                foreach (var file in archive.Entries)
                {
                    if (file.Name.Length == 0)
                        continue;

                    string filePath = Path.Combine(destinationPath, file.Name);
                    CurrentlyExtractedFile?.Invoke(this, file.Name);

                    try
                    {
                        await Task.Run(() => file.ExtractToFile(filePath, overwrite: true));
                    }
                    catch (Exception ex)
                    {
                        failedFiles.Add(new FailedFile(filePath, ex));
                    }

                    extractedCount++;
                    FileExtracted?.Invoke(this, new ExtractInfo(extractedCount, filePath));
                }

                ArchiveExtracted?.Invoke(this, new ZipFileInfo(path, archive.Entries.Select(e => e.Name)));
            }
        }

        return new FinishedExtraction(extractedCount, failedFiles);
    }
}
