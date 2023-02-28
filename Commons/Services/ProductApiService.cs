using Commons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Commons.Services;

public interface IProductApiService : IAuthApiService
{
    Task<Product> GetProduct(int id);

    Task<IEnumerable<Product>> GetAllProducts();
    Task<RestResponse> PostProduct(Product product);
    Task<RestResponse> UpdateProduct(Product product);
    Task<RestResponse> DeleteProduct(int id);
}

public class ProductApiService : BookShopApiServiceBase, IProductApiService
{
    public ProductApiService(RestClient client) : base(client)
    {
    }

    public ProductApiService()
    {
    }

    public async Task<Product> GetProduct(int id)
    {
        var restRequest = new RestRequest($"/Products/api/products/{id}");
        var restResponse = await Client.ExecuteAsync(restRequest, Method.Get);

        return JsonConvert.DeserializeObject<Product>(restResponse.Content);
    }
    
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        var restRequest = new RestRequest($"/Products/api/products/");
        var restResponse = await Client.ExecuteAsync(restRequest, Method.Get);

        return JsonConvert.DeserializeObject<IEnumerable<Product>>(restResponse.Content);
    }
    
    public async Task<RestResponse> PostProduct(Product product)
    {
        var restRequest = new RestRequest("/Products/api/products");
        restRequest.AddJsonBody(product);
        var restResponse = await Client.ExecuteAsync(restRequest, Method.Post);

        var jObj = JObject.Parse(restResponse.Content);

        product.Id = (int)jObj.SelectToken("id")?.Value<int>();

        return restResponse;
    }
    
    public async Task<RestResponse> UpdateProduct(Product product)
    {
        var request = new RestRequest($"/Products/api/products");
            
        request.AddJsonBody(product);
        return await Client.ExecuteAsync(request, Method.Put);
    }
    
    public async Task<RestResponse> DeleteProduct(int id)
    {
        var restRequest = new RestRequest($"/Products/api/products/{id}");
        return await Client.ExecuteAsync(restRequest, Method.Delete);
    }
}