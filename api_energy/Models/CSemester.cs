using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_energy.Models
{
    [Table("Semester")]
    public class CSemester
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")] // Ajusta el tamaño según tu necesidad
        public string name_semester { get; set; }

        [Required]
        public DateTime create_date { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")] // Ajusta el tamaño según tu necesidad
        public string create_username { get; set; }

        [Required]
        public int year { get; set; }

        [Required]
        public byte type { get; set; } // Cambiado a byte para tinyint

        // Relación uno a muchos con Database
        public ICollection<Database> Databases { get; set; }

        // [JsonIgnore]
        // public virtual Database database { get; set; }
    }
}
