using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractToWork.Core;

namespace ExtractToWork.CL
{
    public class ExtractController
    {
        private int _allFilesCount;
        private string _currenlyExtractedFile = "";

        public async Task Extract(string[] args)
        {
            Directory.CreateDirectory(Program.Config.DestinationDir);

            ExtractorAPI extractor = new ExtractorAPI(
                new DirectoryPathGenerator(Program.Config.DestinationDir, DateTimeOffset.Now.ToString("yyyy-MM-dd"), Program.Config.AppendToDir));
            extractor.FilesCount += (_, e) => { _allFilesCount += e; Console.WriteLine($"Extracting {e} files..."); };
            extractor.CurrentlyExtractedFile += (_, file) => _currenlyExtractedFile = file;
            extractor.Progress += (_, progress) => ReportProgress(progress);
            extractor.FinishedArchive += (_, archiveName) => OnFinishedArhive(archiveName);
            extractor.ErroredFiles += (_, e) => IfErroredExist(e);

            Console.CursorVisible = false;

            await extractor.ExtractAllAsync(args);

            Program.SoundPlayer.Done();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n --- DONE ---\nExtracted " + _allFilesCount + " files.");
            Console.ResetColor();
            Console.CursorVisible = true;

            extractor.OpenExplorerWithDir();
        }

        private void OnFinishedArhive(string archiveName)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Program.SoundPlayer.Click();
            Console.CursorTop -= 1;
            Console.Write(new string(' ', Console.BufferWidth));
            Console.WriteLine($"\n--{archiveName}-- | 100%");
            Console.ResetColor();
        }

        private void ReportProgress(int progress)
        {
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.BufferWidth));
            Console.CursorTop -= 1;
            Console.Write(progress + "%  -  " + _currenlyExtractedFile);
        }

        private void IfErroredExist(List<string> errored)
        {
            Console.WriteLine("-ERRORS- Failed to extract some files:\n");
            foreach (var item in errored)
            {
                Console.WriteLine(item + "\n");
            }
        }
    }
}
