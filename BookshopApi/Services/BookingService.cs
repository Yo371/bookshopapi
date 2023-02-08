using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IBookingService
{
    IEnumerable<Booking> GetAllBookings();

    Booking GetBooking(int id);
    
    Booking GetBooking(int id, string role, string loggedId);

    void UpdateBooking(Booking booking);
    
    bool UpdateBooking(Booking booking, string role, string loggedId);

    void CreateBooking(Booking booking);
    
    bool CreateBooking(Booking booking, string role, string loggedId);

    bool DeleteBooking(int id, string role, string loggedId);
}

public class BookingService : IBookingService
{
    private readonly BookShopContext _context;
    
    private const string CustomerRole = "Customer";
    
    public BookingService(BookShopContext context)
    {
        _context = context;
    }

    public IEnumerable<Booking> GetAllBookings()
    {
        return _context.Bookings.Include(e => e.BookingStatus)
            .Include(e => e.ProductEntity)
            .Include(e => e.UserEntity)
            .ThenInclude(e => e.Auth)
            .Select(e => e.Adapt<Booking>()).ToList();
    }
    
    public Booking GetBooking(int id)
    {
        return _context.Bookings.Include(e => e.BookingStatus)
            .Include(e => e.ProductEntity)
            .Include(e => e.UserEntity)
            .ThenInclude(e => e.Auth).First(e => e.Id == id)
            .Adapt<Booking>();
    }

    public Booking GetBooking(int id, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(id.ToString()))
                return GetBooking(id);
            else
                return null;
        }

        return GetBooking(id);
    }

    public void UpdateBooking(Booking booking)
    {
        var bookingEntity = booking.Adapt<BookingEntity>();
        var product = _context.Products.First(e => e.Id == bookingEntity.ProductEntity.Id);
        var user = _context.Users.First(e => e.Id == bookingEntity.UserEntity.Id);
        var bookingStatus = _context.BookingStatuses.First(e => e.Status == bookingEntity.BookingStatus.Status);

        bookingEntity.ProductEntity = product;
        bookingEntity.UserEntity = user;
        bookingEntity.BookingStatus = bookingStatus;

        _context.Bookings.Update(bookingEntity);
        _context.SaveChanges();
    }

    public bool UpdateBooking(Booking booking, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(booking.UserEntity.Id.ToString()))
            {
                UpdateBooking(booking);
                return true;
            }
            else
                return false;
        }

        UpdateBooking(booking);
        return true;
    }

    public void CreateBooking(Booking booking)
    {
        var bookingEntity = booking.Adapt<BookingEntity>();
        var product = _context.Products.First(e => e.Id == bookingEntity.ProductEntity.Id);
        var user = _context.Users.First(e => e.Id == bookingEntity.UserEntity.Id);
        var bookingStatus = _context.BookingStatuses.First(e => e.Status == bookingEntity.BookingStatus.Status);

        bookingEntity.ProductEntity = product;
        bookingEntity.UserEntity = user;
        bookingEntity.BookingStatus = bookingStatus;

        _context.Bookings.Add(bookingEntity);
        _context.SaveChanges();
        booking.Id = bookingEntity.Id;
    }

    public bool CreateBooking(Booking booking, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(booking.UserEntity.Id.ToString()))
            {
                CreateBooking(booking);
                return true;
            }
            else
                return false;
        }

        CreateBooking(booking);
        return true;
    }

    public bool DeleteBooking(int id, string role, string loggedId)
    {
        var booking = _context.Bookings.Find(id);
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(booking.UserEntity.Id.ToString()))
            {
                booking.BookingStatus = new BookingStatusEntity() { Status = Status.Cancelled };
                _context.SaveChanges();
                return true;
            }
            else
                return false;
        }
        
        _context.Bookings.Remove(booking);
        _context.SaveChanges();
        return true;
    }
}
