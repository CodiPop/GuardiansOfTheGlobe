using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GuardiansOfTheGlobeApi.Models
{
    public class VillanoPeleasResult
    {
        public string nombre_villano { get; set; }
        public int cantidad_de_peleas { get; set; }
    }
}
