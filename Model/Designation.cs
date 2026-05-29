using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.api.Model
{
    [Table("designationTbl")]
    public class Designation
    {
        [Key]
        public int designationId { get; set; }

        [Required, MaxLength(50)]
        public string designationName { get; set; } = string.Empty;

        public bool isActive { get; set; }
    }
}