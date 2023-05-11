using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GuardiansOfTheGlobeApi.Models
{
    [Table("agenda")]
    public partial class Agenda
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_heroe")]
        public int IdHeroe { get; set; }
        [Column("fecha", TypeName = "date")]
        public DateTime Fecha { get; set; }
        [Column("evento")]
        [StringLength(255)]
        [Unicode(false)]
        public string Evento { get; set; } = null!;

        [ForeignKey("IdHeroe")]
        [InverseProperty("Agenda")]
        public virtual Hero IdHeroeNavigation { get; set; } = null!;
    }
}
