using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_energy.Models
{
    [Table("Periods")]
    public class Periods
    {
        [Key, Column(Order = 0)]
        [Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string creator_username { get; set; }
        [Required]
        public DateTime date_create { get; set; }
        public DateTime? update_create { get; set; }
        [Required]
        public Boolean active { get; set; }

        [NotMapped]
        public virtual List<Files> files { get; set; }

    }
}

