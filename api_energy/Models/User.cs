using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_energy.Models
{
    [Table("User")]
    public class User
    {
        [Key, Column(Order = 0)]

        [Required]
        public int user_id { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public Boolean active { get; set; }

    }
}

