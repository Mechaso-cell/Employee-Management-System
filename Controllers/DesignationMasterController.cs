using Employee.api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Employee.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationMasterController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public DesignationMasterController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: All Designations
        [HttpGet("GetAllDesignations")]
        public IActionResult GetAllDesignations()
        {
            try
            {
                var data = _context.designations.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: Filter Designations
        [HttpGet("FilterDesignations")]
        public IActionResult FilterDesignations([FromQuery] string? name, [FromQuery] bool? isActive)
        {
            try
            {
                var query = _context.designations.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.designationName.Contains(name));
                }

                if (isActive != null)
                {
                    query = query.Where(x => x.isActive == isActive);
                }

                return Ok(query.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: Add Designation
        [HttpPost("AddDesignation")]
        public IActionResult AddDesignation([FromBody] Designation designation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.designations.Add(designation);
                _context.SaveChanges();

                return Ok("Designation Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: Update Designation
        [HttpPut("UpdateDesignation")]
        public IActionResult UpdateDesignation([FromBody] Designation designation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existing = _context.designations.Find(designation.designationId);

                if (existing == null)
                    return NotFound("Designation not found");

                existing.designationName = designation.designationName;
                existing.isActive = designation.isActive;

                _context.SaveChanges();

                return Ok("Designation Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: Delete Designation
        [HttpDelete("DeleteDesignation/{id}")]
        public IActionResult DeleteDesignation(int id)
        {
            try
            {
                var data = _context.designations.Find(id);

                if (data == null)
                    return NotFound("Designation not found");

                _context.designations.Remove(data);
                _context.SaveChanges();

                return Ok("Designation Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}