using Employee.api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public EmployeeMasterController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: All Employees
        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var data = _context.Employees.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: Filter + Sort + Pagination
        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees(
            [FromQuery] string? name,
            [FromQuery] string? city,
            [FromQuery] int? designationId,
            [FromQuery] string? sortBy,
            [FromQuery] bool isDesc = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = _context.Employees.AsQueryable();

                // FILTER
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(x => x.name.Contains(name));

                if (!string.IsNullOrEmpty(city))
                    query = query.Where(x => x.city.Contains(city));

                if (designationId.HasValue)
                    query = query.Where(x => x.designationId == designationId);

                // SORT
                query = sortBy switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.name) : query.OrderBy(x => x.name),
                    "city" => isDesc ? query.OrderByDescending(x => x.city) : query.OrderBy(x => x.city),
                    "email" => isDesc ? query.OrderByDescending(x => x.email) : query.OrderBy(x => x.email),
                    _ => query.OrderBy(x => x.employeeId)
                };

                // PAGINATION
                var totalRecords = query.Count();

                var data = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    totalRecords,
                    page,
                    pageSize,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: Add Employee (UNIQUE: email + contactNo)
        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee([FromBody] EmployeeModel emp)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var exists = _context.Employees.Any(x =>
                    x.email.ToLower() == emp.email.ToLower() ||
                    x.contactNo == emp.contactNo);

                if (exists)
                    return BadRequest("Email or Contact Number already exists");

                emp.modifiedDate = DateTime.Now;

                _context.Employees.Add(emp);
                _context.SaveChanges();

                return Ok("Employee Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: Update Employee
        [HttpPut("UpdateEmployee")]
        public IActionResult UpdateEmployee([FromBody] EmployeeModel emp)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existing = _context.Employees.Find(emp.employeeId);

                if (existing == null)
                    return NotFound("Employee not found");

                // UNIQUE CHECK (excluding self)
                var exists = _context.Employees.Any(x =>
                    x.employeeId != emp.employeeId &&
                    (x.email.ToLower() == emp.email.ToLower() ||
                     x.contactNo == emp.contactNo));

                if (exists)
                    return BadRequest("Email or Contact Number already exists");

                existing.name = emp.name;
                existing.contactNo = emp.contactNo;
                existing.email = emp.email;
                existing.city = emp.city;
                existing.state = emp.state;
                existing.pincode = emp.pincode;
                existing.altContactNo = emp.altContactNo;
                existing.address = emp.address;
                existing.designationId = emp.designationId;
                existing.modifiedDate = DateTime.Now;

                _context.SaveChanges();

                return Ok("Employee Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: Employee
        [HttpDelete("DeleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var emp = _context.Employees.Find(id);

                if (emp == null)
                    return NotFound("Employee not found");

                _context.Employees.Remove(emp);
                _context.SaveChanges();

                return Ok("Employee Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = _context.Employees
                    .FirstOrDefault(x =>
                        x.email == login.email &&
                        x.contactNo == login.contactNumber);

                if (user == null)
                {
                    return Unauthorized("Invalid email or contact number");
                }

                return Ok(new
                {
                    message = "Login successful",
                    user.employeeId,
                    user.name,
                    user.email,
                    user.contactNo,
                    user.designationId,
                    user.designationName,
                    user.role
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}