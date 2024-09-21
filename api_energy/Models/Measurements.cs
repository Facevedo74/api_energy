using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_energy.Models
{
    [Table("Measurements")]
    public class Measurements
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [ForeignKey("Periods")]
        public int? id_period { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Tiempo { get; set; }

        public int? Periodo { get; set; }

        [StringLength(1)]
        public string Unidad_V { get; set; }

        [StringLength(1)]
        public string Unidad_A { get; set; }

        [StringLength(1)]
        public string Unidad_W { get; set; }

        [StringLength(1)]
        public string Unidad_An { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Tension_L1_L2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Tension_L2_L3 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Tension_L3_L1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Tension_Trifasica_ff { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Tension_L1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Tension_L2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Tension_L3 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Tension_Trifasica { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Corriente_L1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Corriente_L2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Corriente_L3 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Corriente_Trifasica { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Activa_L1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Activa_L2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Activa_L3 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Activa_Trifasica { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Inductiva_L1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Inductiva_L2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Inductiva_L3 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Inductiva_Trifasica { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Capacitiva_L1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Capacitiva_L2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Capacitiva_L3 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Capacitiva_Trifasica { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Factor_Potencia_L1 { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Factor_Potencia_L2 { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Factor_Potencia_L3 { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Factor_Potencia_Trifasica { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Aparente_L1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Aparente_L2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Aparente_L3 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? P_Aparente_Trifasica { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Energias_Energia_III { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Energias_Energia_L_III { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Energias_Energia_C_III { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Distorsion_Armonica_VL1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Distorsion_Armonica_VL2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Distorsion_Armonica_VL3 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Distorsion_Armonica_IL1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Distorsion_Armonica_IL2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Distorsion_Armonica_IL3 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Flicker_Pst_L1 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Flicker_Pst_L2 { get; set; }

        [Column(TypeName = "decimal(10,1)")]
        public decimal? Flicker_Pst_L3 { get; set; }

        public virtual Periods Periods { get; set; }
    }
}
