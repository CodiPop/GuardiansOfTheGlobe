using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardiansOfTheGlobeApi.DBContext;
using GuardiansOfTheGlobeApi.Models;
using Microsoft.Data.SqlClient;

namespace GuardiansOfTheGlobeApi.Controllers
{
    public class VillanosController : Controller
    {
        private readonly AppDbContext _context;

        public VillanosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Villanos
        public async Task<IActionResult> Index()
        {
              return _context.Villanos != null ? 
                          View(await _context.Villanos.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Villanos'  is null.");
        }
        [HttpGet("villano")]
        public async Task<ActionResult<IEnumerable<Villano>>> GetVillanos()
        {
            return await _context.Villanos.ToListAsync();
        }
        [HttpGet("BuscarNombreVillano/{nombre}")]
        public async Task<ActionResult<IEnumerable<Villano>>> GetVillanoNombre(string nombre)
        {
            var busqueda = await _context.Villanos.Where(p => p.Nombre.Contains(nombre)).ToListAsync();


            if (busqueda == null)
            {
                return NotFound();
            }

            return busqueda;
        }
        [HttpGet("BuscarOrigenVillano/{origen}")]
        public async Task<ActionResult<IEnumerable<Villano>>> GetVillanosOrigen(string origen)
        {
            var busqueda = await _context.Villanos.Where(p => p.Origen.Contains(origen)).ToListAsync();


            if (busqueda == null)
            {
                return NotFound();
            }

            return busqueda;
        }
        [HttpGet("BuscarDebilidadVillano/{debilidad}")]
        public async Task<ActionResult<IEnumerable<Villano>>> GetVillanoDebilidad(string debilidad)
        {
            var busqueda = await _context.Villanos.Where(p => p.Debilidades.Contains(debilidad)).ToListAsync();


            if (busqueda == null)
            {
                return NotFound();
            }

            return busqueda;
        }
        [HttpGet("villanos/VmasPeleas/{idheroe}")]
        public async Task<IActionResult> GetVillanosMas(int idheroe)
        {
    
            var peleas = _context.Peleas;
            var villanos = _context.Villanos;

            var id_heroe = idheroe; 

            var query = (from p in peleas
                         join v in villanos on p.IdVillano equals v.Id
                         where p.IdHeroe == id_heroe
                         group v by new { v.Id, v.Nombre } into g
                         orderby g.Count() descending
                         select new
                         {
                             nombre_villano = g.Key.Nombre,
                             cantidad_de_peleas = g.Count()
                         }).Take(1);
            var results = await query.FirstOrDefaultAsync();

            


            return Ok(results);



           
        }
        // GET: Villanos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Villanos == null)
            {
                return NotFound();
            }

            var villano = await _context.Villanos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (villano == null)
            {
                return NotFound();
            }

            return View(villano);
        }

        // GET: Villanos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Villanos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost("CrearVillano")]
        public async Task<IActionResult> CrearVillano([FromBody] Villano villanoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parameters = new[]
            {
        new SqlParameter("@nombre", villanoModel.Nombre),
        new SqlParameter("@edad", villanoModel.Edad),
        new SqlParameter("@habilidades", villanoModel.Habilidades),
        new SqlParameter("@origen", villanoModel.Origen),
        new SqlParameter("@debilidades", villanoModel.Debilidades),
        new SqlParameter("@poder", villanoModel.Poder)
    };

            await _context.Database.ExecuteSqlRawAsync("EXEC InsertarNuevoVillano @nombre, @edad, @habilidades, @origen, @debilidades, @poder", parameters);

            return Ok("Registro creado exitosamente");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Edad,Habilidades,Origen,Poder,Debilidades")] Villano villano)
        {
            if (ModelState.IsValid)
            {
                _context.Add(villano);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(villano);
        }

        // GET: Villanos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Villanos == null)
            {
                return NotFound();
            }

            var villano = await _context.Villanos.FindAsync(id);
            if (villano == null)
            {
                return NotFound();
            }
            return View(villano);
        }
        [HttpPut("ActualizarVillano/{id}")]
        public async Task<IActionResult> ActualizarVillano(int id, [FromBody] Villano villanoModel)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var villanoExistente = await _context.Villanos.FindAsync(id);

            if (villanoExistente == null)
            {
                return NotFound();
            }

            // Actualizar los campos del villano existente con los valores proporcionados
            villanoExistente.Nombre = villanoModel.Nombre;
            villanoExistente.Edad = villanoModel.Edad;
            villanoExistente.Habilidades = villanoModel.Habilidades;
            villanoExistente.Origen = villanoModel.Origen;
            villanoExistente.Debilidades = villanoModel.Debilidades;
            villanoExistente.Poder = villanoModel.Poder;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Villano actualizado exitosamente");
        }


        // POST: Villanos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Edad,Habilidades,Origen,Poder,Debilidades")] Villano villano)
        {
            if (id != villano.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(villano);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VillanoExists(villano.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(villano);
        }

        // GET: Villanos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Villanos == null)
            {
                return NotFound();
            }

            var villano = await _context.Villanos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (villano == null)
            {
                return NotFound();
            }

            return View(villano);
        }

        [HttpDelete("BorrarVillano/{id}")]
        public async Task<IActionResult> BorrarVillano(int id)
        {
            var villano = await _context.Villanos.FindAsync(id);

            if (villano == null)
            {
                return NotFound();
            }

            _context.Villanos.Remove(villano);
            await _context.SaveChangesAsync();

            return Ok("Villano borrado exitosamente");
        }

        // POST: Villanos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Villanos == null)
            {
                return Problem("Entity set 'AppDbContext.Villanos'  is null.");
            }
            var villano = await _context.Villanos.FindAsync(id);
            if (villano != null)
            {
                _context.Villanos.Remove(villano);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet("VillanoMasDerrotas")]
        public IActionResult GetVillanoMasDerrotas()
        {
            var resultado = (from p in _context.Peleas
                             join v in _context.Villanos on p.IdVillano equals v.Id
                             join h in _context.Heroes on p.IdHeroe equals h.Id
                             where h.Edad < 18 && p.Resultado == "Victoria"
                             group v by v.Nombre into grupoVillano
                             orderby grupoVillano.Count() descending
                             select new
                             {
                                 Villano = grupoVillano.Key,
                                 CantidadDerrotas = grupoVillano.Count()
                             }).FirstOrDefault();

            return Ok(resultado);
        }


        private bool VillanoExists(int id)
        {
          return (_context.Villanos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
