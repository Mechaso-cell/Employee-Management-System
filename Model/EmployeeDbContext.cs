using Microsoft.EntityFrameworkCore;

namespace Employee.api.Model
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }
        public EmployeeDbContext()
        {

        }
       
    }
}
