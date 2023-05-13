﻿using System;
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

        // POST: Heroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        private bool HeroExists(int id)
        {
          return (_context.Heroes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
