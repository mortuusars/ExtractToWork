using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ExtractToWork.Core
{
    public interface IArchiveExtractor
    {
        event EventHandler<int> FilesCount;
        event EventHandler<int> Progress;
        event EventHandler<string> CurrentlyExtractedFile;
        event EventHandler<string> LastExtractedFile;
        event EventHandler<string> ErrorMessage;
        event EventHandler<List<string>> ErroredFiles;

        /// <summary>
        /// Extracts archive to provided directory.
        /// </summary>
        Task<bool> ExtractAsync(string archiveFilepath, string destinationDir);
    }

    public class ZipExtractor : IArchiveExtractor
    {
        public event EventHandler<int> FilesCount;
        public event EventHandler<int> Progress;
        public event EventHandler<string> CurrentlyExtractedFile;
        public event EventHandler<string> LastExtractedFile;
        public event EventHandler<string> ErrorMessage;
        public event EventHandler<List<string>> ErroredFiles;

        private int _filesCount;
        private List<string> _erroredFiles;
        private string _destinationDir;

        /// <summary>
        /// Extracts archive to provided directory.
        /// </summary>
        public async Task<bool> ExtractAsync(string archiveFilepath, string destinationDir)
        {
            using ZipArchive zipArchive = await Task.Run(() => ZipFile.OpenRead(archiveFilepath));

            if (zipArchive.Entries.Count == 0)
            {
                ErrorMessage?.Invoke(this, "No files in the archive.");
                zipArchive.Dispose();
                return false;
            }

            _filesCount = zipArchive.Entries.Count;
            FilesCount?.Invoke(this, zipArchive.Entries.Count);

            await ExtractFilesAsync(zipArchive, destinationDir);

            return true;
        }

        private async Task ExtractFilesAsync(ZipArchive zipArchive, string destinationDir)
        {
            _destinationDir = destinationDir;
            _erroredFiles = new List<string>();

            Directory.CreateDirectory(_destinationDir);

            foreach (var file in zipArchive.Entries)
            {
                await ExtractSingleFile(file);
                NotifyInfo(zipArchive, file);
            }

            if (_erroredFiles.Count > 0)
                ErroredFiles?.Invoke(this, _erroredFiles);
        }

        private async Task ExtractSingleFile(ZipArchiveEntry file)
        {
            CurrentlyExtractedFile?.Invoke(this, file.Name);

            try
            {
                await Task.Run(() => file.ExtractToFile(_destinationDir + file.Name, overwrite: true));
            }

            catch (Exception)
            {
                //Console.WriteLine("ERROR --------------------------------");
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(file.Name);
                //Console.WriteLine(file.FullName);
                if (file.Length > 0)
                    _erroredFiles.Add(file.Name);
            }

        }

        private void NotifyInfo(ZipArchive zipArchive, ZipArchiveEntry file)
        {
            LastExtractedFile?.Invoke(this, file.Name);
            double index = zipArchive.Entries.IndexOf(file);
            int progress = (int)((index / _filesCount) * 100);
            Progress?.Invoke(this, progress);
        }
    }
}
