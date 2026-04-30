using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.api.Model
{
    [Table("department1Tbl")]
    public class Department
    {
        public int departmentId { get; set; }
        public string departmentName { get; set; } = string.Empty;
    }
}
