using BookshopApi.Services;
using Commons.Services;
using RestSharp;
using Tests.Models;

namespace Tests.ApiTests;

public class BaseTest
{
    protected readonly UserApiService UserApiService = new UserApiService(new RestClient(Constants.BaseUrl));
    
    protected readonly ProductApiService ProductApiService = new ProductApiService(new RestClient(Constants.BaseUrl));
}