using kiwiproject.frameiograbber.Model;

namespace kiwiproject.frameiograbber
{
    public class FrameIoDownloader
    {
        private FrameIoApiClient client;
        private readonly string fileTypeFilter;
        private readonly string downloadRootPath;

        public FrameIoDownloader(string frameIoToken, string fileTypeToDownload, string downloadRootPath)
        {
            client = new FrameIoApiClient(frameIoToken);
            this.fileTypeFilter = fileTypeToDownload;
            this.downloadRootPath = downloadRootPath ?? throw new ArgumentNullException(nameof(downloadRootPath));
        }

        public async Task DownloadLoop()
        {
            var accounts = await client.GetAccounts();
            var account = accounts.First();
            Console.WriteLine($"Account: {account.DisplayName}");
            Console.WriteLine($"Storage: {account.Storage / 1024 / 1024 / (float)1024:F2} of {account.Subscription.TotalStorageLimit / 1024 / 1024 / 1024:F0} GB used");

            var teams = await client.GetTeams(account);
            var team = teams.First();
            Console.WriteLine($"Team: {team.Name}");

            var projects = await client.GetProjects(team);
            var project = projects.First();
            Console.WriteLine($"Project: {project.Name}");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"Getting assets...");
                var assets = await GetAssetsList(project.RootAssetId);

                var images = assets
                    .Where(a => a.FileType == fileTypeFilter || fileTypeFilter == "")
                    .Where(a => a.Status == "transcoded" || a.Status == "uploaded")
                    .ToList();

                Console.WriteLine();
                Console.WriteLine($"Found {images.Count} images ready to download");
                Console.WriteLine();

                var waiting = assets
                    .Where(a => a.FileType == fileTypeFilter || fileTypeFilter == "")
                    .Where(a => a.Status != "transcoded" && a.Status != "uploaded")
                    .ToList();

                if (images.Any())
                {
                    Console.WriteLine("Downloading...");
                    Console.WriteLine();
                    foreach (var image in images)
                    {
                        var filePath = downloadRootPath + image._path;
                        var data = await client.GetFile(image);
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        File.WriteAllBytes(filePath, data);
                        await client.DeleteAsset(image);
                        Console.Write(".");
                    }
                }

                Console.WriteLine();
                if (!waiting.Any())
                {
                    Console.WriteLine("Nothing waiting, sleeping 1 minute");
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
                else
                {
                    Console.WriteLine($"{waiting.Count} waiting, sleeping 15 seconds");
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
                Console.WriteLine();
            }
        }

        public async Task<List<Asset>> GetAssetsList(string rootAssetId, string path = "", List<Asset> list = null)
        {
            list ??= new List<Asset>();

            var assets = await client.GetAssets(rootAssetId);
            foreach (var asset in assets)
            {
                asset._path = path + "\\" + asset.Name;
                if (asset.ItemCount > 0)
                {
                    await GetAssetsList(asset.Id, asset._path, list);
                }
                else if (asset.FileType != null)
                {
                    Console.WriteLine($"{asset._path} \t {asset.Status}");
                    list.Add(asset);
                }
            }

            return list;
        }
    }
}