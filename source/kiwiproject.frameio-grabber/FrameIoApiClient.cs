using Flurl.Http;
using kiwiproject.frameiograbber.Model;

namespace kiwiproject.frameiograbber
{
    public class FrameIoApiClient
    {
        private const string apiUrl = "https://api.frame.io/v2/";
        private readonly string token;

        public FrameIoApiClient(string frameIoToken)
        {
            if(string.IsNullOrWhiteSpace(frameIoToken)) throw new ArgumentNullException(nameof(frameIoToken));
            
            this.token = frameIoToken;
        }

        public async Task DeleteAsset(Asset asset)
        {
            await (apiUrl + "assets/" + asset.Id)
                .WithOAuthBearerToken(token)
                .DeleteAsync();
        }

        public async Task<byte[]> GetFile(Asset asset)
        {
            var result = await (asset.Original)
                .WithOAuthBearerToken(token)
                .GetBytesAsync();

            return result;
        }

        public async Task<T> Get<T>(string endpoint)
        {
            var result = await (apiUrl + endpoint)
                .WithOAuthBearerToken(token)
                .GetJsonAsync<T>();

            return result;
        }

        public Task<List<Account>> GetAccounts()
            => Get<List<Account>>("accounts");

        public Task<List<Team>> GetTeams(Account account)
            => Get<List<Team>>($"accounts/{account.Id}/teams");

        public Task<List<Project>> GetProjects(Team team)
            => Get<List<Project>>($"teams/{team.Id}/projects");

        public Task<List<Asset>> GetAssets(string rootAssetId)
            => Get<List<Asset>>($"assets/{rootAssetId}/children");
    }
}