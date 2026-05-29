using Employee.api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public DepartmentMasterController(EmployeeDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllDepartments")]
        public IActionResult GetDepartment()
        {
            var deptList = _context.departments.ToList();

            return Ok(deptList);
        }
    }
}