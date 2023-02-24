using Commons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Commons.Services;

public interface IBookingApiService : IAuthApiService
{
    Booking GetBooking(int id);
    IEnumerable<Booking> GetAllBookings(int idOfRelatedToBookingUser = 0);
    RestResponse PostBooking(Booking booking);
    RestResponse UpdateBooking(Booking booking);
    RestResponse DeleteBooking(int id);
    string Authenticate(UserLogin userLogin);
}

public class BookingApiService : BookShopApiServiceBase, IBookingApiService
{
    public BookingApiService(RestClient client, string login, string password) : base(client, login, password)
    {
    }

    public BookingApiService(RestClient client) : base(client)
    {
    }

    public BookingApiService()
    {
    }

    public Booking GetBooking(int id)
    {
        var restRequest = new RestRequest($"/Booking/api/bookings/{id}");
        var restResponse = Client.Execute(restRequest, Method.Get);

        return JsonConvert.DeserializeObject<Booking>(restResponse.Content);
    }
    
    public IEnumerable<Booking> GetAllBookings(int idOfRelatedToBookingUser = 0)
    {
        var restRequest = new RestRequest($"/Booking/api/bookings/");
        var restResponse = Client.Execute(restRequest, Method.Get);

        var allBookings = JsonConvert.DeserializeObject<IEnumerable<Booking>>(restResponse.Content);

        return idOfRelatedToBookingUser != 0 ? allBookings.Where(e => e.User.Id == idOfRelatedToBookingUser) : allBookings;
    }
    
    public RestResponse PostBooking(Booking booking)
    {
        var restRequest = new RestRequest("/Booking/api/bookings");
        restRequest.AddJsonBody(booking);
        var restResponse = Client.Execute(restRequest, Method.Post);

        var jObj = JObject.Parse(restResponse.Content);

        booking.Id = (int)jObj.SelectToken("id")?.Value<int>();

        return restResponse;
    }
    
    public RestResponse UpdateBooking(Booking booking)
    {
        var request = new RestRequest($"/Booking/api/bookings");
            
        request.AddJsonBody(booking);
        return Client.Execute(request, Method.Put);
    }
    
    public RestResponse DeleteBooking(int id)
    {
        var restRequest = new RestRequest($"/Booking/api/bookings/{id}");
        return Client.Execute(restRequest, Method.Delete);
    }
}