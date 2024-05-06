using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using U2U_AI.API.Requests;
using U2U_AI.Infra;
using U2U_AI.Infra.Entities;

namespace U2U_AI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
  private readonly CourseDbContext _context;

  public BookingsController(CourseDbContext context)
  {
    _context = context;
  }

  // GET: api/Bookings
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
  {
    return await _context.Bookings.ToListAsync();
  }

  // GET: api/Bookings/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Booking>> GetBooking(int id)
  {
    var booking = await _context.Bookings.FindAsync(id);

    if (booking == null)
    {
      return NotFound();
    }

    return booking;
  }

  // PUT: api/Bookings/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutBooking(int id, Booking booking)
  {
    if (id != booking.Id)
    {
      return BadRequest();
    }

    _context.Entry(booking).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!BookingExists(id))
      {
        return NotFound();
      }
      else
      {
        throw;
      }
    }

    return NoContent();
  }

  // POST: api/Bookings
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<Booking>> PostBooking(BookingRequest bookingRequest)
  {
    Session session = _context.Sessions.SingleOrDefault(s => s.Id == bookingRequest.SessionId);
    Booking booking = new Booking() 
    { 
      FirstName = bookingRequest.FirstName,
      LastName = bookingRequest.LastName,
      Session = session,
      SessionId = session.Id
    };

    _context.Bookings.Add(booking);
    await _context.SaveChangesAsync();

    return Created();
  }

  // DELETE: api/Bookings/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteBooking(int id)
  {
    var booking = await _context.Bookings.FindAsync(id);
    if (booking == null)
    {
      return NotFound();
    }

    _context.Bookings.Remove(booking);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool BookingExists(int id)
  {
    return _context.Bookings.Any(e => e.Id == id);
  }
}
