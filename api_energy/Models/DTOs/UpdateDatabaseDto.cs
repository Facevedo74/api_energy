using System.ComponentModel.DataAnnotations;

namespace api_energy.Models.DTOs
{
    public class UpdateDatabaseDto
    {
        [Required] // Campo requerido
        public int Id { get; set; } // ID del registro a actualizar

        [Required] // Campo requerido
        public int Id_Semester { get; set; } // ID del semestre

        [Required] // Campo requerido
        public long NIS { get; set; } // NIS del registro

        [MaxLength(255)] // Límite de caracteres
        public string NombreArchivo { get; set; } // Nombre del archivo

        [MaxLength(50)] // Límite de caracteres
        public string Medidor { get; set; } // Medidor

        [MaxLength(100)] // Límite de caracteres
        public string Provincia { get; set; } // Provincia

        [MaxLength(100)] // Límite de caracteres
        public string Corregimiento { get; set; } // Corregimiento

        [MaxLength(100)] // Límite de caracteres
        public string Categoria_Tarifaria { get; set; } // Categoría tarifaria

        [MaxLength(100)] // Límite de caracteres
        public string Departamento { get; set; } // Departamento
    }
}
