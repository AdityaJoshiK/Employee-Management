using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management.Models
{
    public class EmployeeModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeID { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    public int DesignationID { get; set; }

    public int? DepartmentID { get; set; }  // Nullable field

    public decimal? Salary { get; set; }    // Nullable field

    [Required]
    public DateTime JoiningDate { get; set; }

    public int? ReportingPersonID { get; set; }  // Nullable field

    public DateTime? CreationDate { get; set; }  // Nullable field

    public DateTime? ModificationDate { get; set; }  // Nullable field

    [MaxLength]
    public string PhotoPath { get; set; }  // Max length for nvarchar(max)
}
}