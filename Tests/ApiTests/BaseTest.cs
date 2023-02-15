using BookshopApi.Models;
using RestSharp;
using Tests.Models;
using Tests.Services;

namespace Tests.ApiTests;

public class BaseTest
{
    protected readonly UserApiService UserApiService = new UserApiService(new RestClient(Constants.BaseUrl), "goga", "123");
    
    protected readonly ProductApiService ProductApiService = new ProductApiService(new RestClient(Constants.BaseUrl), "manager", "123");
}