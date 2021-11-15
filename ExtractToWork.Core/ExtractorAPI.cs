using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractToWork.Core
{
    public interface IExtractorAPI
    {
        Task<List<string>> ExtractAllAsync(string[] paths, bool createDirectoryForFinished = true);
        void OpenExplorerWithDir();
    }

    public class ExtractorAPI : IExtractorAPI
    {
        public event EventHandler<int>? FilesCount;
        public event EventHandler<int>? Progress;
        public event EventHandler<string>? CurrentlyExtractedFile;
        public event EventHandler<string>? LastExtractedFile;
        public event EventHandler<string>? FinishedArchive;
        public event EventHandler<List<string>>? ErroredFiles;

        private List<string> extractedFiles = new();
        private List<string> erroredFiles = new();

        private DirectoryPathGenerator _dirPathGenerator;

        public ExtractorAPI(DirectoryPathGenerator dirPathGenerator)
        {
            _dirPathGenerator = dirPathGenerator;
        }

        public async Task<List<string>> ExtractAllAsync(string[] paths, bool createDirectoryForFinished = true)
        {
            foreach (var path in paths)
            {
                string archiveName = Path.GetFileNameWithoutExtension(path);
                string directoryPath = _dirPathGenerator.CreatePath(archiveName);

                await ExtractAndNotify(path, directoryPath);
                if (createDirectoryForFinished)
                    Directory.CreateDirectory(_dirPathGenerator.CreatePath(archiveName, appendToEnd: false));
            }
            return extractedFiles;
        }

        public void OpenExplorerWithDir()
        {
            Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", _dirPathGenerator.CreatePath("", false));
        }

        private async Task ExtractAndNotify(string archiveFilepath, string destinationDirectory)
        {
            IArchiveExtractor extractor = CreateExtractor();

            await extractor.ExtractAsync(archiveFilepath, destinationDirectory);

            FinishedArchive?.Invoke(this, Path.GetFileName(archiveFilepath));
        }

        private IArchiveExtractor CreateExtractor()
        {
            IArchiveExtractor extractor = new ZipExtractor();

            extractor.FilesCount += (_, e) => FilesCount?.Invoke(this, e);
            extractor.Progress += (_, e) => Progress?.Invoke(this, e);
            extractor.CurrentlyExtractedFile += (_, e) => CurrentlyExtractedFile?.Invoke(this, e);
            extractor.LastExtractedFile += (_, e) =>
            {
                extractedFiles.Add(e);
                LastExtractedFile?.Invoke(this, e);
            };
            extractor.ErroredFiles += (_, errored) =>
            {
                if (errored.Count > 0)
                {
                    erroredFiles.AddRange(errored);
                    ErroredFiles?.Invoke(this, errored);
                }
            };

            return extractor;
        }
    }
}
