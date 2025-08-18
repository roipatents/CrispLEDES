using ObjCRuntime;

namespace FreshbooksLedesConverter;

public partial class PreferencesViewController(NativeHandle handle) : NSViewController(handle)
{
    partial void ChooseConfigurationFile(NSObject sender)
    {
        Common.ChooseConfigFile();
    }

    partial void ChooseCsvDirectory(NSObject sender)
    {
        Common.ChooseDefaultCsvDirectory();
    }
}
