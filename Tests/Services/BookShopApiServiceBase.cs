using RestSharp;
using RestSharp.Authenticators;

namespace Tests.Services;

public class BookShopApiServiceBase
{
    protected readonly RestClient Client;

    public BookShopApiServiceBase(RestClient client, string login, string password)
    {
        Client = client;

        Client.Authenticator = new HttpBasicAuthenticator(login, password);
        Client.AddDefaultHeader("accept", "text/plain");
    }
}