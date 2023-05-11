using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GuardiansOfTheGlobeApi.Models
{
    [Table("patrocinadores")]
    public partial class Patrocinador
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_heroe")]
        public int IdHeroe { get; set; }
        [Column("nombre")]
        [StringLength(50)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        [Column("monto")]
        public double Monto { get; set; }
        [Column("origen_dinero")]
        [StringLength(255)]
        [Unicode(false)]
        public string? OrigenDinero { get; set; }

        [ForeignKey("IdHeroe")]
        [InverseProperty("Patrocinadores")]
        public virtual Hero IdHeroeNavigation { get; set; } = null!;
    }
}
