using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

public class IGDBApi
{
    private const string BaseUrl = "https://id.twitch.tv/oauth2/token";
    private const string ApiUrl = "https://api.igdb.com/v4";
    private readonly string _clientId;
    private readonly string _clientSecret;
    private string _accessToken;

    public IGDBApi(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
    }
    /// <summary>
    /// This function send the access request and return the access token
    /// </summary>
    /// <returns></returns>
    public async Task<string> AuthenticateAsync()
    {
        RestClient client = new RestClient(BaseUrl);
        RestRequest request = new RestRequest(Method.POST);

        request.AddParameter("client_id", _clientId);
        request.AddParameter("client_secret", _clientSecret);
        request.AddParameter("grant_type", "client_credentials");

        IRestResponse response = await Task.FromResult(client.Execute(request));

        JToken json = JToken.Parse(response.Content);
        string accessToken = json.Value<string>("access_token");
        _accessToken = accessToken;

        return accessToken;

        /*
         * IRestResponse response = await client.ExecuteAsync(request);
          if (response.IsSuccessful)
         {
             JObject jsonResponse = JObject.Parse(response.Content);
             _accessToken = jsonResponse["access_token"].ToString();
             return true;
         }

         return false;
         */
    }
    /// <summary>
    /// This function makes requerst asynchronously
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    private async Task<string> MakeRequestAsync(string endpoint, string query)
    {
        RestClient client = new RestClient(ApiUrl);
        RestRequest request = new RestRequest(endpoint, Method.POST, DataFormat.Json);

        var res = AuthenticateAsync();
        

        request.AddHeader("Client-ID", _clientId);
        request.AddHeader("Authorization", $"Bearer {_accessToken}");
        request.AddParameter("text/plain", query, ParameterType.RequestBody);

        IRestResponse response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return response.Content;
        }

        return null;
    }
    /// <summary>
    /// This function search for games in the api
    /// </summary>
    /// <param name="searchTerm"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<string> SearchGamesAsync(string searchTerm, int limit = 10)//limit = 10
    {
        //name,  summary, platforms.name, release_dates.date,
        string query = $@"
            search ""{searchTerm}""; 
            fields id, name,url,first_release_date, summary, cover.url; 
            limit {limit};";
        return await MakeRequestAsync("games", query);
    }
    //"img-responsive cover_uniform game-card-image cover_big cover_big"
    /// <summary>
    /// This function is used to get the game cover
    /// </summary>
    /// <param name="gameId">the game id </param>
    /// <returns></returns>
    public async Task<string> GetGameCoverAsync(int gameId)
    {
        string endpoint = "covers";
        string query = $@"where game = {gameId};
                        fields url;
                        limit 1;";
        string result = await MakeRequestAsync(endpoint, query);
        JArray jsonArray = JArray.Parse(result);
        string imageUrl = null;
        if (jsonArray.Count > 0)
        {
            imageUrl = jsonArray[0].Value<string>("url");
        }
        return imageUrl;
    }
}