﻿using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class IGDBApi
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

        public async Task<string> SearchGamesAsync(string searchTerm, int limit = 10)
        {
            string query = $@"
            search ""{searchTerm}""; 
            fields name, platforms.name, release_dates.date, summary; 
            limit {limit};";
            return await MakeRequestAsync("games", query);
        }
    }
}
