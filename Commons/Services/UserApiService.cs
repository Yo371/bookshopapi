﻿using Commons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Commons.Services;

public interface IUserApiService : IAuthApiService
{
    User GetUser(int id);
    IEnumerable<User> GetAllUsers();
    RestResponse PostUser(User user);
    RestResponse UpdateUser(User user);
    RestResponse DeleteUser(int id);
}

public class UserApiService : BookShopApiServiceBase, IUserApiService
{
    public UserApiService(RestClient client, string login, string password) : base(client, login, password)
    {
    }

    public UserApiService(RestClient client) : base(client)
    {
    }

    public UserApiService()
    {
    }


    public User GetUser(int id)
    {
        var restRequest = new RestRequest($"/User/api/users/{id}");
        var restResponse = Client.Execute(restRequest, Method.Get);

        return JsonConvert.DeserializeObject<User>(restResponse.Content);
    }

    public IEnumerable<User> GetAllUsers()
    {
        var restRequest = new RestRequest($"/User/api/users/");
        var restResponse = Client.Execute(restRequest, Method.Get);
        var content = restResponse.Content;

        return JsonConvert.DeserializeObject<IEnumerable<User>>(restResponse.Content);
    }

    public RestResponse PostUser(User user)
    {
        var restRequest = new RestRequest("/User/api/users");
        restRequest.AddJsonBody(user);
        var restResponse = Client.Execute(restRequest, Method.Post);

        var jObj = JObject.Parse(restResponse.Content);

        user.Id = (int)jObj.SelectToken("id")?.Value<int>();

        return restResponse;
    }
    
    public RestResponse UpdateUser(User user)
    {
        var request = new RestRequest($"/User/api/users");
            
        request.AddJsonBody(user);
        return Client.Execute(request, Method.Put);
    }
    
    public RestResponse DeleteUser(int id)
    {
        var restRequest = new RestRequest($"/User/api/users/{id}");
        return Client.Execute(restRequest, Method.Delete);
    }
}