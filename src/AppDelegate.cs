namespace FreshbooksLedesConverter;

[Register(nameof(AppDelegate))]
public class AppDelegate : NSApplicationDelegate
{
    public override void DidFinishLaunching(NSNotification notification)
    {
        Common.SetDefaultConfigFileIfNecessary();
    }

    public override void WillTerminate(NSNotification notification)
    {
        // Insert code here to tear down your application
    }

    public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
    {
        return true;
    }

    public override bool OpenFile(NSApplication sender, string filename)
    {
        MainViewController.Current?.OpenFile(filename);
        return true;
    }
}
