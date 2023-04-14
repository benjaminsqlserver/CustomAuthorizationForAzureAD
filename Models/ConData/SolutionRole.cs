using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadzenBlazorServerADDemo.Models.ConData
{
    [Table("SolutionRoles", Schema = "dbo")]
    public partial class SolutionRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string RoleName { get; set; }

        public ICollection<SolutionUsersInRole> SolutionUsersInRoles { get; set; }

    }
}