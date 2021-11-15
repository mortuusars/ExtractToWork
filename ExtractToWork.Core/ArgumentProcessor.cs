using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractToWork.Core
{
    public interface IArgumentProcessor
    {
        bool IsAllArgsAreFilePaths(string[] args);
    }

    public class ArgumentProcessor : IArgumentProcessor
    {
        public bool IsAllArgsAreFilePaths(string[] args)
        {
            if (args.Length == 0)
                return false;

            return args.All(arg => Path.IsPathFullyQualified(arg));
        }
    }
}
