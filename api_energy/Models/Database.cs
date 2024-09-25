using api_energy.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


[Table("Database")]
public class Database
{

    [Key]
    public int Id { get; set; }

    [ForeignKey("Semester")]
    public int Id_Semester { get; set; }

    [Required]
    public long NIS { get; set; }

    [Required]
    [StringLength(255)]
    public string NombreArchivo { get; set; }

    [Required]
    [StringLength(100)]
    public string Medidor { get; set; }

    [Required]
    [StringLength(100)]
    public string Provincia { get; set; }

    [Required]
    [StringLength(100)]
    public string Corregimiento { get; set; }

    [Required]
    [StringLength(100)]
    public string Categoria_Tarifaria { get; set; }

    [Required]
    [StringLength(100)]
    public string Departamento { get; set; }

    [Required]
    public bool Active { get; set; } = true;

    [JsonIgnore]
    public virtual CSemester Semester { get; set; } 
}
