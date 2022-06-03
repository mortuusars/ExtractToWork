using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractToWork.Core;

public interface IPathGenerator
{
    string Create(string archiveName, bool useModifier = true);
}

public class PathGenerator : IPathGenerator
{
    private readonly string _baseDirectoryPath;
    private readonly Func<string, string> _extractedFolderNameModifier;

    public PathGenerator(string baseDirectoryPath, Func<string, string> extractedFolderNameModifier)
    {
        _baseDirectoryPath = baseDirectoryPath;
        _extractedFolderNameModifier = extractedFolderNameModifier;
    }

    public string Create(string archiveName, bool useModifier = true)
    {
        return useModifier ? Path.Combine(_baseDirectoryPath, _extractedFolderNameModifier(archiveName)) :
            Path.Combine(_baseDirectoryPath, archiveName);
    }
}
