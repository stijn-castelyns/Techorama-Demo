using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U2U_AI.Infra.Entities;
public class Session
{
  public int Id { get; set; }
  public Course Course { get; set; }
  public string CourseId { get; set; }
  public List<DateOnly> OrganisationDays { get; set; }
  public List<Booking> Bookings { get; set; }
}
