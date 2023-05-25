using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GuardiansOfTheGlobeApi.DBContext;
using GuardiansOfTheGlobeApi.Models;

namespace GuardiansOfTheGlobeApi.Controllers
{
    public class HeroesController : Controller
    {
        private readonly AppDbContext _context;

        public HeroesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Heroes
        public async Task<IActionResult> Index()
        {
              return _context.Heroes != null ? 
                          View(await _context.Heroes.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Heroes'  is null.");
        }

        [HttpGet("heroe")]
        public async Task<ActionResult<IEnumerable<Hero>>> GetAlumnos()
        {
            return await _context.Heroes.ToListAsync();
        }


        [HttpGet("BuscarNombre/{nombre}")]
        public async Task<ActionResult<IEnumerable<Hero>>> GetHeroesNombre(string nombre)
        {
            var busqueda = await _context.Heroes.Where(p => p.Nombre.Contains(nombre)).ToListAsync();

            
            if (busqueda == null)
            {
                return NotFound();
            }

            return busqueda;
        }
        [HttpGet("BuscarRelacion/{relacion}")]
        public async Task<ActionResult<IEnumerable<Hero>>> GetHeroesRelacion(string relacion)
        {
            var busqueda = await _context.Heroes.Where(p => p.RelacionesPersonales.Contains(relacion)).ToListAsync();


            if (busqueda == null)
            {
                return NotFound();
            }

            return busqueda;
        }
        [HttpGet("BuscarHabilidad/{habilidad}")]
        public async Task<ActionResult<IEnumerable<Hero>>> GetHeroesHabilidad(string habilidad)
        {
            var busqueda = await _context.Heroes.Where(p => p.Habilidades.Contains(habilidad)).ToListAsync();


            if (busqueda == null)
            {
                return NotFound();
            }

            return busqueda;
        }


        [HttpGet("HeroesPorEdad")]
        public async Task<IActionResult> ObtenerHeroesPorEdad()
        {
            var heroesPorEdad = from h in _context.Heroes
                                group h by new
                                {
                                    GrupoEdad = h.Edad >= 18 ? "Mayores de Edad" : "Adolescentes",
                                    h.Id
                                } into grupoEdad
                                select new
                                {
                                    GrupoEdad = grupoEdad.Key.GrupoEdad,
                                    Id = grupoEdad.Key.Id
                                };



            return Ok(heroesPorEdad);
        }


        [HttpGet("heroes/MyG")]
        public async Task<IActionResult> GetMyG()


        {
            var resultado =  from pelea in _context.Peleas
                            join heroe in _context.Heroes on pelea.IdHeroe equals heroe.Id
                            join villano in _context.Villanos on pelea.IdVillano equals villano.Id
                            where heroe.Nombre == "The Guardians" || heroe.Nombre == "Mark"
                            select new
                            {
                                nombre_heroe = heroe.Nombre,
                                nombre_villano = villano.Nombre,
                                resultado = pelea.Resultado
                            };

            return Ok(resultado);
        }
        [HttpGet("heroes/Top3")]
        public async Task<IActionResult> GetTop()


        {
            var resultado = (from pelea in _context.Peleas
                             join heroe in _context.Heroes on pelea.IdHeroe equals heroe.Id
                             where pelea.Resultado == "Victoria"
                             group heroe by heroe.Nombre into g
                             orderby g.Count() descending
                             select new
                             {
                                 nombre = g.Key,
                                 victorias = g.Count()
                             }).Take(3);


            return Ok(resultado);
        }
        // GET: Heroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Heroes == null)
            {
                return NotFound();
            }

            var hero = await _context.Heroes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hero == null)
            {
                return NotFound();
            }

            return View(hero);
        }

        // GET: Heroes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPut("ActualizarHeroe/{id}")]
        public async Task<IActionResult> ActualizarHeroe(int id, [FromBody] Hero heroeModel)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var heroeExistente = await _context.Heroes.FindAsync(id);

            if (heroeExistente == null)
            {
                return NotFound();
            }

            // Actualizar los campos del héroe existente con los valores proporcionados
            heroeExistente.Nombre = heroeModel.Nombre;
            heroeExistente.Edad = heroeModel.Edad;
            heroeExistente.Habilidades = heroeModel.Habilidades;
            heroeExistente.Debilidades = heroeModel.Debilidades;
            heroeExistente.RelacionesPersonales = heroeModel.RelacionesPersonales;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Registro actualizado exitosamente");
        }


        [HttpPost("CrearHeroe")]
        public async Task<IActionResult> CrearHeroe([FromBody] Hero heroeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obtener los valores de los parámetros desde heroeModel
            string nombre = heroeModel.Nombre;
            int edad = (int)heroeModel.Edad;
            string habilidades = heroeModel.Habilidades;
            string debilidades = heroeModel.Debilidades;
            string relacionesPersonales = heroeModel.RelacionesPersonales;

            // Ejecutar el procedimiento almacenado
            await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC InsertarNuevoHeroe 
        @nombre = {nombre}, 
        @edad = {edad},
        @habilidades = {habilidades},
        @debilidades = {debilidades},
        @relaciones_personales = {relacionesPersonales}");

            return Ok("Registro creado exitosamente");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Edad,Habilidades,Debilidades,RelacionesPersonales")] Hero hero)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hero);
        }

        // GET: Heroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Heroes == null)
            {
                return NotFound();
            }

            var hero = await _context.Heroes.FindAsync(id);
            if (hero == null)
            {
                return NotFound();
            }
            return View(hero);
        }

        // POST: Heroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Edad,Habilidades,Debilidades,RelacionesPersonales")] Hero hero)
        {
            if (id != hero.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeroExists(hero.Id))
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
            return View(hero);
        }

        // GET: Heroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Heroes == null)
            {
                return NotFound();
            }

            var hero = await _context.Heroes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hero == null)
            {
                return NotFound();
            }

            return View(hero);
        }

        // POST: Heroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Heroes == null)
            {
                return Problem("Entity set 'AppDbContext.Heroes'  is null.");
            }
            var hero = await _context.Heroes.FindAsync(id);
            if (hero != null)
            {
                _context.Heroes.Remove(hero);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete("BorrarHeroe/{id}")]
        public async Task<IActionResult> BorrarHeroe(int id)
        {
            var heroe = await _context.Heroes.FindAsync(id);

            if (heroe == null)
            {
                return NotFound();
            }

            _context.Heroes.Remove(heroe);
            await _context.SaveChangesAsync();

            return Ok("Héroe borrado exitosamente");
        }


        private bool HeroExists(int id)
        {
          return (_context.Heroes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
