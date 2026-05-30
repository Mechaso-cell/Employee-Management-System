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

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] Department dept)
        {
            _context.departments.Add(dept);
            _context.SaveChanges();

            return Ok("Department Added Successfully");
        }

        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] Department dept)
        {
            var existingDept = _context.departments.Find(dept.departmentId);

            if (existingDept == null)
            {
                return NotFound("Department not found");
            }

            existingDept.departmentName = dept.departmentName;
            existingDept.isActive = dept.isActive;

            _context.SaveChanges();

            return Ok("Department Updated Successfully");
        }

        [HttpDelete("DeleteDepartment/{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var dept = _context.departments.Find(id);

            if (dept == null)
            {
                return NotFound("Department not found");
            }

            _context.departments.Remove(dept);

            _context.SaveChanges();

            return Ok("Department Deleted Successfully");
        }
    }
}