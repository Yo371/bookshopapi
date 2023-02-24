using Commons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace Commons.Services;

public interface IBookShopApiServiceBase
{
    void SetClient(RestClient client);
}

public class BookShopApiServiceBase : IBookShopApiServiceBase
{
    protected RestClient Client;

    public BookShopApiServiceBase(RestClient client, string login, string password)
    {
        Client = client;

        //Client.Authenticator = new HttpBasicAuthenticator(login, password);
        Client.AddDefaultHeader("accept", "text/plain");
    }
    
    public BookShopApiServiceBase(RestClient client)
    {
        Client = client;
    }

    public BookShopApiServiceBase()
    {
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

            Client.Authenticator = new JwtAuthenticator(token);
            Client.AddDefaultHeader("accept", "text/plain");

            return token;
        }
        return null;
    }
    
    public void AddTokenToHeader(string token)
    {
        Client.Authenticator = new JwtAuthenticator(token);
        Client.AddDefaultHeader("accept", "text/plain");
    }
    
    public void SetClient(RestClient client)
    {
        Client = client;
    }
}