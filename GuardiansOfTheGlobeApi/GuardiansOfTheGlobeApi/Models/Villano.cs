using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GuardiansOfTheGlobeApi.Models
{
    [Table("villanos")]
    public partial class Villano
    {
        public Villano()
        {
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
        public int Edad { get; set; }
        [Column("habilidades")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Habilidades { get; set; }
        [Column("origen")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Origen { get; set; }
        [Column("poder")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Poder { get; set; }
        [Column("debilidades")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Debilidades { get; set; }

        [InverseProperty("IdVillanoNavigation")]
        public virtual ICollection<Pelea> Peleas { get; set; }
    }
}
