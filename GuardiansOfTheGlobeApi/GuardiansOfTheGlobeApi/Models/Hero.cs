using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GuardiansOfTheGlobeApi.Models
{
    [Table("heroes")]
    public partial class Hero
    {
        public Hero()
        {
            Agenda = new HashSet<Agenda>();
            Patrocinadores = new HashSet<Patrocinador>();
            Peleas = new HashSet<Pelea>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre")]
        [StringLength(50)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        [Column("edad")]
        public int? Edad { get; set; }
        [Column("habilidades")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Habilidades { get; set; }
        [Column("debilidades")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Debilidades { get; set; }
        [Column("relaciones_personales")]
        [StringLength(255)]
        [Unicode(false)]
        public string? RelacionesPersonales { get; set; }

        [InverseProperty("IdHeroeNavigation")]
        public virtual ICollection<Agenda> Agenda { get; set; }
        [InverseProperty("IdHeroeNavigation")]
        public virtual ICollection<Patrocinador> Patrocinadores { get; set; }
        [InverseProperty("IdHeroeNavigation")]
        public virtual ICollection<Pelea> Peleas { get; set; }
    }
}
