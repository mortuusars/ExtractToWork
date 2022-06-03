using System.IO;
using System.Text.Json;
using System.Windows;
using PureConfig;

namespace ExtractToWork.WPF;

public class Config : ConfigBase
{
    public string DestinationPath { get => GetValue("C:\\"); set => SetValue(value); }
    public string AppendText { get => GetValue("RAW"); set => SetValue(value); }
    public bool CreateEmptyFolder { get => GetValue(true); set => SetValue(value); }
    public bool CreateCurrentDateFolder { get => GetValue(true); set => SetValue(value); }
    public bool CloseWhenCompleted { get => GetValue(false); set => SetValue(value); }
    public bool OpenDestinationFolderWhenCompleted { get => GetValue(false); set => SetValue(value); }
    public bool AlwaysOnTop { get => GetValue(true); set => SetValue(value); }

    public static string FilePath { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), App.AppName, "config.json");

    public Config()
    {
        this.PropertyChanged += (s, e) => Save();
    }

    public void Load()
    {
        try
        {
            string json = File.ReadAllText(FilePath);
            Dictionary<string, object?> properties = new JsonConfigDeserializer().Deserialize<Config>(json);
            SetProperties(properties);
        }
        catch (DirectoryNotFoundException) { }
        catch (FileNotFoundException) { }
        catch (Exception ex)
        {
            var type = ex.GetType();
            MessageBox.Show($"Config was not loaded: '{ex.Message}'", App.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void Save()
    {
        try
        {
            this.Serialize(new JsonConfigSerializer(new JsonSerializerOptions() { WriteIndented = true }))?
                .WriteToFile(FilePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Config was not saved: '{ex.Message}'", App.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
