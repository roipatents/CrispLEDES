/*
 * The MIT License (MIT)
 *
 * Copyright © 2022-2025, Richardson Oliver Insights, LLC, All Rights Reserved
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
namespace CrispLEDES;

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
