namespace FreshbooksLedesConverter;

public static class Common
{
    private const string ConfigFileKey = "configfile";

    private const string DefaultCsvDirectoryKey = "lastdirectory";

    public static string? ChooseConfigFile()
    {
        var chooser = NSOpenPanel.OpenPanel;
        chooser.AllowedContentTypes =
        [
            UniformTypeIdentifiers.UTTypes.Text
        ];
        chooser.AllowsMultipleSelection = false;
        chooser.AllowsOtherFileTypes = false;
        chooser.CanChooseDirectories = false;
        chooser.CanChooseFiles = true;
        chooser.CanCreateDirectories = false;
        chooser.ShowsHiddenFiles = false;
        chooser.Title = "Select the Configuration File";
        var currentPath = ConfigFile;
        if (!string.IsNullOrWhiteSpace(currentPath) && File.Exists(currentPath))
        {
            chooser.DirectoryUrl = new NSUrl(currentPath, false);
        }

        if (chooser.RunModal() != (int)NSModalResponse.OK)
        {
            return null;
        }
            
        var fileName = chooser.Url.Path;
        NSUserDefaults.StandardUserDefaults.SetString(fileName, ConfigFileKey);
        return fileName;
    }

    public static string? ChooseDefaultCsvDirectory()
    {
        var chooser = NSOpenPanel.OpenPanel;
        chooser.AllowsMultipleSelection = false;
        chooser.CanChooseDirectories = true;
        chooser.CanChooseFiles = false;
        chooser.CanCreateDirectories = true;
        chooser.ShowsHiddenFiles = false;
        chooser.Title = "Select the Default CSV Directory";
        var currentPath = DefaultCsvDirectory;
        if (!string.IsNullOrWhiteSpace(currentPath) && Directory.Exists(currentPath))
        {
            chooser.DirectoryUrl = new NSUrl(currentPath, true);
        }

        if (chooser.RunModal() != (int)NSModalResponse.OK)
        {
            return null;
        }

        var fileName = chooser.Url.Path;
        NSUserDefaults.StandardUserDefaults.SetString(fileName, DefaultCsvDirectoryKey);
        return fileName;
    }

    public static string? ConfigFile => NSUserDefaults.StandardUserDefaults.StringForKey(ConfigFileKey);

    public static string? DefaultCsvDirectory => NSUserDefaults.StandardUserDefaults.StringForKey(DefaultCsvDirectoryKey);

    public static string SetDefaultConfigFileIfNecessary()
    {
        var currentPath = ConfigFile;
        if (!string.IsNullOrWhiteSpace(currentPath) && File.Exists(currentPath))
        {
            return currentPath;
        }
        currentPath = NSBundle.MainBundle.PathForResource("configuration-file", "txt");
        NSUserDefaults.StandardUserDefaults.SetString(currentPath, ConfigFileKey);
        return currentPath;
    }
}
