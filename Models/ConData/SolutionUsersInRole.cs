using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RadzenBlazorServerADDemo.Models.ConData
{
    [Table("SolutionUsersInRoles", Schema = "dbo")]
    public partial class SolutionUsersInRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long UserID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int RoleID { get; set; }

        public SolutionRole SolutionRole { get; set; }

        public SolutionUser SolutionUser { get; set; }

    }
}