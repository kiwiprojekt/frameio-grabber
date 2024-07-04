using Microsoft.Extensions.Configuration;

namespace kiwiproject.frameiograbber
{

    internal class Program
    {
        static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.local.json", optional: true);

            var appSettings = builder.Build().Get<AppSettings>()!;

            try
            {
                var downloader = new FrameIoDownloader(appSettings.FrameIoToken, appSettings.FileTypeFilter, appSettings.DownloadRootPath);

                await downloader.DownloadLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

    }

}