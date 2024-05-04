using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prueba_redarbor.Models
{
    [Table("User")]
    public class User
    {
        [Key, Column(Order = 0)]
        [Required]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Fax { get; set; }
        public string? Name { get; set; }
        public DateTime? LastLogin { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int PortalId { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int StatusId { get; set; }
        public string? Telephone { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [Required]
        public string Username { get; set; }
    }
}

