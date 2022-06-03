using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ExtractToWork.Core;

public static class Utils
{
    /// <summary>
    /// Removes extension from input string. Returns empty string if input is empty.
    /// </summary>
    /// <exception cref="ArgumentNullException">When input is null</exception>
    public static string RemoveFileExtension(this string filename)
    {
        if (filename == null)
            throw new ArgumentNullException(nameof(filename), "Input cannot be null.");

        if (string.IsNullOrWhiteSpace(filename))
            return string.Empty;

        var rgx = new Regex(".+\\\\");
        string directory = rgx.IsMatch(filename) ? rgx.Match(filename).Value : "";


        string filenameWithourDir = directory.Length > 0 ? filename.Replace(directory, "") : filename;

        string extension = Regex.Match(filenameWithourDir, "\\.\\w+$").Value;
        if (extension.Length == 0)
            return filename;

        string filenameWithoutExt = filenameWithourDir.Replace(extension, "");
        return directory + filenameWithoutExt;
    }

    public static void OpenFolderInExplorer(string directoryPath)
    {
        Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", directoryPath);
    }
}
