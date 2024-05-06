using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U2U_AI.Infra;
internal class CourseDbContextFactory : IDesignTimeDbContextFactory<CourseDbContext>
{
  public CourseDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<CourseDbContext>();
    optionsBuilder.UseSqlServer("Server=tcp:sc-techorama-demo.database.windows.net,1433;Initial Catalog=U2U;Persist Security Info=False;User ID=trainer;Password=P@ssword123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    return new CourseDbContext(optionsBuilder.Options);
  }
}
