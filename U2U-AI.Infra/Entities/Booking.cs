using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U2U_AI.Infra.Entities;
public class Booking
{
  public int Id { get; set; }
  public Session Session { get; set; }
  public int SessionId { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
}
