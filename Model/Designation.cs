using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.api.Model
{
    public class Designation
    {
        [Table("department1Tbl")]
        public class Department
        {
            public int departmentId { get; set; }
            [Required, MaxLength(50)]
            public string departmentName { get; set; } = string.Empty;

            public bool isActive { get; set; }
        }
    }
}

