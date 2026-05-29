using Microsoft.EntityFrameworkCore;

namespace Employee.api.Model
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> departments { get; set; }

        public DbSet<Designation> designations { get; set; }
    }
}