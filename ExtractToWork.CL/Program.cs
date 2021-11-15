using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using ExtractToWork.Core;

namespace ExtractToWork.CL
{
    static class Program
    {
        public const string CFG_PATH = "cfg.json";
        public static Config Config { get; set; }
        public static Sounds SoundPlayer;

        static Program()
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            Config = Config.Read(CFG_PATH);
            Config.Save(CFG_PATH);

            SoundPlayer = new Sounds();
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("ExtractToWork | args:");
            args.ToList().ForEach(x => Console.WriteLine(x));
            Console.WriteLine();


            if (args.Length == 0)
            {
                SoundPlayer.Click();
                Console.WriteLine("Available commands:");
                Console.WriteLine("-dir path  -Change output directory.");
                Console.WriteLine("-a text  -Change appended text. Encase in double quotes for text with spaces.");
                Console.WriteLine("exit   - To exit");

                while (!EnteringCommands())
                {

                }
                return;
            }

            try
            {
                IArgumentProcessor argProc = new ArgumentProcessor();
                if (!new ArgumentProcessor().IsAllArgsAreFilePaths(args))
                {
                    Console.WriteLine("Paths are not valid.");
                }

                await new ExtractController().Extract(args);
                CloseAfter(3);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }

        private static void CloseAfter(double seconds)
        {
            Timer timer = new Timer(seconds * 1000);
            timer.Elapsed += (s, e) => Environment.Exit(0);
            timer.Start();
        }

        #region Console Commands

        private static bool EnteringCommands()
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return false;

            if (input == "exit")
                Environment.Exit(0);

            string command = "";

            if (Regex.IsMatch(input, "^-dir"))
                command = "-dir";
            else if (Regex.IsMatch(input, "^-a"))
                command = "-a";

            string parameter;

            try
            {
                parameter = input.Replace(command, "").Trim().Replace("\"", "");
            }
            catch (Exception)
            {

                parameter = "";
            }

            try
            {
                
                return ExecuteCommand(command, parameter);
            }
            catch (Exception)
            {
                Console.WriteLine("Command is not recognized.");
                Console.ReadKey();
            }

            return false;
        }

        private static bool ExecuteCommand(string command, string parameter)
        {
            switch (command)
            {
                case "-dir":
                {
                    if (string.IsNullOrWhiteSpace(parameter))
                        Console.WriteLine("Current export directory is set to: " + Config.DestinationDir);
                    else if (Path.IsPathFullyQualified(parameter))
                    {
                        Config.DestinationDir = parameter;
                        Config.Save(CFG_PATH);
                        Console.WriteLine("Export directory is changed to: " + Config.DestinationDir);
                    }
                    return false;
                }
                case "-a":
                {
                    if (string.IsNullOrWhiteSpace(parameter))
                        Console.WriteLine("Appended text is: " + "\"" + Config.AppendToDir + "\"" + " (without quotes)");
                    else
                    {
                        Config.AppendToDir = parameter;
                        Config.Save(CFG_PATH);
                        Console.WriteLine("Appended text is changed to: " + "\"" + Config.AppendToDir + "\"" + " (without quotes)");
                    }
                    return false;
                }
                case "exit":
                {
                    Environment.Exit(0);
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
