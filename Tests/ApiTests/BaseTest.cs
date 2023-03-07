using BookshopApi.Services;
using Commons.Services;
using Framework.Lib.Configuration;
using RestSharp;
using Tests.Models;

namespace Tests.ApiTests;

public class BaseTest
{

    protected readonly UserApiService UserApiService = new UserApiService(new RestClient(ConfigManager.Options.BookshopUrlApi));
    
    protected readonly ProductApiService ProductApiService = new ProductApiService(new RestClient(ConfigManager.Options.BookshopUrlApi));
}