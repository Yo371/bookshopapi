using Commons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Commons.Services;

public interface IProductApiService : IAuthApiService
{
    Product GetProduct(int id);

    IEnumerable<Product> GetAllProducts();
    RestResponse PostProduct(Product product);
    RestResponse UpdateProduct(Product product);
    RestResponse DeleteProduct(int id);
    string Authenticate(UserLogin userLogin);
}

public class ProductApiService : BookShopApiServiceBase, IProductApiService
{
    public ProductApiService(RestClient client, string login, string password) : base(client, login, password)
    {
    }

    public ProductApiService(RestClient client) : base(client)
    {
    }

    public ProductApiService()
    {
    }

    public Product GetProduct(int id)
    {
        var restRequest = new RestRequest($"/Products/api/products/{id}");
        var restResponse = Client.Execute(restRequest, Method.Get);

        return JsonConvert.DeserializeObject<Product>(restResponse.Content);
    }
    
    public IEnumerable<Product> GetAllProducts()
    {
        var restRequest = new RestRequest($"/Products/api/products/");
        var restResponse = Client.Execute(restRequest, Method.Get);

        return JsonConvert.DeserializeObject<IEnumerable<Product>>(restResponse.Content);
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