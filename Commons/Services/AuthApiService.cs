using Commons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace Commons.Services;

public interface IAuthApiService : IBookShopApiServiceBase
{
    string Authenticate(UserLogin userLogin);
    void AddTokenToHeader(string token);
}

public class AuthApiService : IAuthApiService
{
    protected RestClient Client;
    public AuthApiService(RestClient client)
    {
        Client = client;
    }

    public AuthApiService()
    {
    }
    
    public void SetClient(RestClient client)
    {
        Client = client;
    }

    public string Authenticate(UserLogin userLogin)
    {
        var restRequest = new RestRequest("Account/api/token");
        restRequest.AddJsonBody(userLogin);
        var restResponse = Client.Execute(restRequest, Method.Post);

        if (restResponse.IsSuccessStatusCode)
        {
            var jObj = JObject.Parse(restResponse.Content);
            var token = jObj.SelectToken("access_token")?.Value<string>();
        
            return token;
        }

        return null;
    }
    
    public void AddTokenToHeader(string token)
    {
        Client.Authenticator = new JwtAuthenticator(token);
        Client.AddDefaultHeader("accept", "text/plain");
    }
}