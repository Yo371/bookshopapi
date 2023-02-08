using BookshopApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Tests.Services;

public class ProductApiService : BookShopApiServiceBase
{
    public ProductApiService(RestClient client, string login, string password) : base(client, login, password)
    {
    }
    
    public Product GetProduct(int id)
    {
        var restRequest = new RestRequest($"/Products/api/products/{id}");
        var restResponse = Client.Execute(restRequest, Method.Get);

        return JsonConvert.DeserializeObject<Product>(restResponse.Content);
    }
    
    public RestResponse PostProduct(Product product)
    {
        var restRequest = new RestRequest("/Products/api/products");
        restRequest.AddJsonBody(product);
        var restResponse = Client.Execute(restRequest, Method.Post);

        var jObj = JObject.Parse(restResponse.Content);

        product.Id = (int)jObj.SelectToken("id")?.Value<int>();

        return restResponse;
    }
    
    public RestResponse UpdateProduct(Product product)
    {
        var request = new RestRequest($"/Products/api/products");
            
        request.AddJsonBody(product);
        return Client.Execute(request, Method.Put);
    }
    
    public RestResponse DeleteProduct(int id)
    {
        var restRequest = new RestRequest($"/Products/api/products/{id}");
        return Client.Execute(restRequest, Method.Delete);
    }
}