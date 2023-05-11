using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GuardiansOfTheGlobeApi.Models
{
    [Table("peleas")]
    public partial class Pelea
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_heroe")]
        public int IdHeroe { get; set; }
        [Column("id_villano")]
        public int IdVillano { get; set; }
        [Column("resultado")]
        [StringLength(50)]
        [Unicode(false)]
        public string Resultado { get; set; } = null!;

        [ForeignKey("IdHeroe")]
        [InverseProperty("Peleas")]
        public virtual Hero IdHeroeNavigation { get; set; } = null!;
        [ForeignKey("IdVillano")]
        [InverseProperty("Peleas")]
        public virtual Villano IdVillanoNavigation { get; set; } = null!;
    }
}
