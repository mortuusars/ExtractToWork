using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ExtractToWork.Core
{
    public class DirectoryPathGenerator
    {
        private readonly string _baseDirectoryPath;
        private readonly string _appendDirectory;
        private readonly string _appendToEnd;

        /// <summary>
        /// Generates directory path. 
        /// </summary>
        /// <param name="baseDirectoryPath"></param>
        /// <param name="appendDirectory"></param>
        /// <param name="appendToEnd">Will be appended to archiveName</param>
        public DirectoryPathGenerator(string baseDirectoryPath, string appendDirectory = "", string appendToEnd = "")
        {
            if (string.IsNullOrWhiteSpace(baseDirectoryPath))
                throw new ArgumentNullException(nameof(baseDirectoryPath), "Base Path cannot be null or empty.");

            if (!Path.IsPathFullyQualified(baseDirectoryPath))
                throw new ArgumentException("Base Path should be a valid path.", nameof(baseDirectoryPath));

            _baseDirectoryPath = baseDirectoryPath;
            _appendDirectory = appendDirectory.Trim();
            _appendToEnd = appendToEnd;
        }

        /// <summary>
        /// Returns concatenated path consisting of "basePath" + "date in format: 2021-07-24" + Archive Name + end (if specified). Ends in a backslash.
        /// </summary>
        /// <param name="archiveName">Filename. Extension will be removed.</param>
        /// <returns></returns>
        public string CreatePath(string archiveName, bool appendToEnd = true)
        {
            string archiveNoExt = Utils.RemoveFileExtension(archiveName.Trim());
            string path = @$"{_baseDirectoryPath}\{_appendDirectory}\{archiveNoExt}{_appendToEnd}\";
            if (!appendToEnd && !string.IsNullOrEmpty(_appendToEnd))
                path = path.Replace(_appendToEnd, "");
            return Regex.Replace(path, @"\\+", @"\");
        }
    }
}
