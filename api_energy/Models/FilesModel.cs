using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_energy.Models
{
    [Table("Files")]
    public class Files
    {
        [Key, Column(Order = 0)]
        [Required]
        public int id { get; set; }
        [Required]
        public int id_period { get; set; }
        [Required]
        public string name_file { get; set; }

        [JsonIgnore]
        public virtual Periods Periods { get; set; }

    }
}

