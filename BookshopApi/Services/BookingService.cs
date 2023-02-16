using BookshopApi.DataAccess;
using BookshopApi.Entities;
using BookshopApi.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BookshopApi.Services;

public interface IBookingService
{
    Task<IEnumerable<Booking>> GetAllBookingsAsync();

    Task<Booking> GetBookingAsync(int id);

    Task<Booking> GetBookingAsync(int id, string role, string loggedId);

    Task UpdateBookingAsync(Booking booking);

    Task<bool> UpdateBookingAsync(Booking booking, string role, string loggedId);

    Task CreateBookingAsync(Booking booking);

    Task<bool> CreateBookingAsync(Booking booking, string role, string loggedId);

    Task<bool> DeleteBookingAsync(int id, string role, string loggedId);
}

public class BookingService : IBookingService
{
    private readonly BookShopContext _context;

    private const string CustomerRole = "Customer";

    public BookingService(BookShopContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
    {
        try
        {
            return await _context.Bookings.Include(e => e.BookingStatus)
                .Include(e => e.ProductEntity)
                .Include(e => e.UserEntity)
                .ThenInclude(e => e.Auth)
                .Select(e => e.Adapt<Booking>()).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all bookings.", ex);
        }
    }

    public async Task<Booking> GetBookingAsync(int id)
    {
        try
        {
            var bookingEnity = await _context.Bookings.Include(e => e.BookingStatus)
                .Include(e => e.ProductEntity)
                .Include(e => e.UserEntity)
                .ThenInclude(e => e.Auth).FirstOrDefaultAsync(e => e.Id == id);

            return bookingEnity.Adapt<Booking>();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the booking with ID {id}.", ex);
        }
    }

    public async Task<Booking> GetBookingAsync(int id, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(id.ToString()))
                return await GetBookingAsync(id);
            else
                return null;
        }

        return await GetBookingAsync(id);
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        try
        {
            var bookingEntity = booking.Adapt<BookingEntity>();
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == bookingEntity.ProductEntity.Id);
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == bookingEntity.UserEntity.Id);
            var bookingStatus =
                await _context.BookingStatuses.FirstOrDefaultAsync(e => e.Status == bookingEntity.BookingStatus.Status);

            bookingEntity.ProductEntity = product;
            bookingEntity.UserEntity = user;
            bookingEntity.BookingStatus = bookingStatus;

            _context.Bookings.Update(bookingEntity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating booking.", ex);
        }
    }

    public async Task<bool> UpdateBookingAsync(Booking booking, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(booking.UserEntity.Id.ToString()))
            {
                await UpdateBookingAsync(booking);
                return true;
            }
            else
                return false;
        }

        await UpdateBookingAsync(booking);
        return true;
    }

    public async Task CreateBookingAsync(Booking booking)
    {
        try
        {
            var bookingEntity = booking.Adapt<BookingEntity>();
            var product = await _context.Products.FirstOrDefaultAsync(e => e.Id == bookingEntity.ProductEntity.Id);
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Id == bookingEntity.UserEntity.Id);
            var bookingStatus =
                await _context.BookingStatuses.FirstOrDefaultAsync(e => e.Status == bookingEntity.BookingStatus.Status);

            bookingEntity.ProductEntity = product;
            bookingEntity.UserEntity = user;
            bookingEntity.BookingStatus = bookingStatus;

            _context.Bookings.Add(bookingEntity);
            await _context.SaveChangesAsync();
            booking.Id = bookingEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating booking.", ex);
        }
    }

    public async Task<bool> CreateBookingAsync(Booking booking, string role, string loggedId)
    {
        if (role.Equals(CustomerRole))
        {
            if (loggedId.Equals(booking.UserEntity.Id.ToString()))
            {
                await CreateBookingAsync(booking);
                return true;
            }
            else
                return false;
        }

        await CreateBookingAsync(booking);
        return true;
    }

    public async Task<bool> DeleteBookingAsync(int id, string role, string loggedId)
    {
        try
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (role.Equals(CustomerRole))
            {
                if (loggedId.Equals(booking?.UserEntity.Id.ToString()))
                {
                    booking.BookingStatus = new BookingStatusEntity() { Status = Status.Cancelled };
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                    return false;
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting booking.", ex);
        }
    }
}