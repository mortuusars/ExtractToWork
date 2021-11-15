using System;
using System.IO;
using System.Text.Json;

namespace ExtractToWork.Core
{
    public class Config
    {
        public string DestinationDir { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public string AppendToDir { get; set; } = " RAW";

        public bool Save(string path)
        {
            try
            {
                var json = JsonSerializer.Serialize(this);
                File.WriteAllTextAsync(path, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Config Read(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<Config>(json) ?? new Config();
            }
            catch (Exception)
            {
                return new Config();
            }
        }
    }
}
