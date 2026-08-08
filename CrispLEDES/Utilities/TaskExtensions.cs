namespace CrispLEDES.Utilities;

public static class TaskExtensions
{
    extension(Task source)
    {
        public async void FireAndForget()
        {
            try
            {
                await source.ConfigureAwait(true);
            }
            catch
            {
                // ignored
            }
        }
    }
}
