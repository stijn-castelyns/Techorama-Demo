using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using U2U_AI.Infra;
using U2U_AI.Infra.Entities;

namespace U2U_AI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
  private readonly CourseDbContext _context;

  public SessionsController(CourseDbContext context)
  {
    _context = context;
  }

  // GET: api/Sessions
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
  {
    return await _context.Sessions.ToListAsync();
  }

  [HttpGet("{courseSlug}")]
  [SwaggerOperation(Summary = "Retrieve future sessions for a specific course")]
  public async Task<ActionResult<IEnumerable<Session>>> GetFutureSessionsForCourse([SwaggerParameter(Description = "The slugified name of the course")]string courseSlug)
  {
    List<Session> sessions = await _context.Sessions
                         .Where(s => s.CourseId == courseSlug)
                         .ToListAsync();
    return sessions;
  }

  

  // PUT: api/Sessions/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutSession(int id, Session session)
  {
    if (id != session.Id)
    {
      return BadRequest();
    }

    _context.Entry(session).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!SessionExists(id))
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

  // POST: api/Sessions
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<Session>> PostSession(Session session)
  {
    _context.Sessions.Add(session);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetSession", new { id = session.Id }, session);
  }

  // DELETE: api/Sessions/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteSession(int id)
  {
    var session = await _context.Sessions.FindAsync(id);
    if (session == null)
    {
      return NotFound();
    }

    _context.Sessions.Remove(session);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool SessionExists(int id)
  {
    return _context.Sessions.Any(e => e.Id == id);
  }
}
