using System.ComponentModel.DataAnnotations;

namespace api_energy.Models.DTOs
{
    public class UpdateDatabaseDto
    {
        [Required] 
        public int Id { get; set; } 

        [Required] 
        public int Id_Semester { get; set; } 

        [Required] 
        public long NIS { get; set; } 

        [MaxLength(255)] 
        public string NombreArchivo { get; set; } 

        [MaxLength(50)] 
        public string Medidor { get; set; } 

        [MaxLength(100)] 
        public string Provincia { get; set; } 

        [MaxLength(100)] 
        public string Corregimiento { get; set; } 

        [MaxLength(100)] 
        public string Categoria_Tarifaria { get; set; } 

        [MaxLength(100)] 
        public string Departamento { get; set; } 
    }
}
