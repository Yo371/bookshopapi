using Commons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Commons.Services;

public interface IUserApiService : IAuthApiService
{
    Task<User> GetUser(int id);
    Task<IEnumerable<User>> GetAllUsers();
    Task<RestResponse> PostUser(User user);
    Task<RestResponse> UpdateUser(User user);
    Task<RestResponse> DeleteUser(int id);
}

public class UserApiService : BookShopApiServiceBase, IUserApiService
{
    public UserApiService(RestClient client) : base(client)
    {
    }

    public UserApiService()
    {
    }


    public async Task<User> GetUser(int id)
    {
        var restRequest = new RestRequest($"/User/api/users/{id}");
        var restResponse = await Client.ExecuteAsync(restRequest, Method.Get);
        
        return JsonConvert.DeserializeObject<User>(restResponse.Content);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var restRequest = new RestRequest($"/User/api/users/");
        
        var restResponse = await Client.ExecuteAsync(restRequest, Method.Get);

        return JsonConvert.DeserializeObject<IEnumerable<User>>(restResponse.Content);;
    }

    public async Task<RestResponse> PostUser(User user)
    {
        var restRequest = new RestRequest("/User/api/users");
        restRequest.AddJsonBody(user);
        var restResponse = await Client.ExecuteAsync(restRequest, Method.Post);

        var jObj = JObject.Parse(restResponse.Content);

        user.Id = (int)jObj.SelectToken("id")?.Value<int>();

        return restResponse;
    }
    
    public async Task<RestResponse> UpdateUser(User user)
    {
        var request = new RestRequest($"/User/api/users");
            
        request.AddJsonBody(user);
        return await Client.ExecuteAsync(request, Method.Put);
    }
    
    public async Task<RestResponse> DeleteUser(int id)
    {
        var restRequest = new RestRequest($"/User/api/users/{id}");
        return await Client.ExecuteAsync(restRequest, Method.Delete);
    }
}