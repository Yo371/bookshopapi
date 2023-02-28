using Commons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Commons.Services;

public interface IBookingApiService : IAuthApiService
{
    Task<Booking> GetBooking(int id);
    Task<IEnumerable<Booking>> GetAllBookings(int idOfRelatedToBookingUser = 0);
    Task<RestResponse> PostBooking(Booking booking);
    Task<RestResponse> UpdateBooking(Booking booking);
    Task<RestResponse> DeleteBooking(int id);
}

public class BookingApiService : BookShopApiServiceBase, IBookingApiService
{
    public BookingApiService(RestClient client) : base(client)
    {
    }

    public BookingApiService()
    {
    }

    public async Task<Booking> GetBooking(int id)
    {
        var restRequest = new RestRequest($"/Booking/api/bookings/{id}");
        var restResponse = await Client.ExecuteAsync(restRequest, Method.Get);

        return JsonConvert.DeserializeObject<Booking>(restResponse.Content);
    }
    
    public async Task<IEnumerable<Booking>> GetAllBookings(int idOfRelatedToBookingUser = 0)
    {
        
        var restRequest = new RestRequest($"/Booking/api/bookings/");
        
        var restResponse = await Client.ExecuteAsync(restRequest, Method.Get);

        var allBookings = JsonConvert.DeserializeObject<IEnumerable<Booking>>(restResponse.Content);

        return idOfRelatedToBookingUser != 0 ? allBookings.Where(e => e.User.Id == idOfRelatedToBookingUser) : allBookings;
    }
    
    public async Task<RestResponse> PostBooking(Booking booking)
    {
        var restRequest = new RestRequest("/Booking/api/bookings");
        restRequest.AddJsonBody(booking);
        var restResponse = await Client.ExecuteAsync(restRequest, Method.Post);

        var jObj = JObject.Parse(restResponse.Content);

        booking.Id = (int)jObj.SelectToken("id")?.Value<int>();

        return restResponse;
    }
    
    public async Task<RestResponse> UpdateBooking(Booking booking)
    {
        var request = new RestRequest($"/Booking/api/bookings");
            
        request.AddJsonBody(booking);
        return await Client.ExecuteAsync(request, Method.Put);
    }
    
    public async Task<RestResponse> DeleteBooking(int id)
    {
        var restRequest = new RestRequest($"/Booking/api/bookings/{id}");
        return await Client.ExecuteAsync(restRequest, Method.Delete);
    }
}