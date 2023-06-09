﻿using System;
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
    public class PatrocinadoresController : Controller
    {
        private readonly AppDbContext _context;

        public PatrocinadoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Patrocinadores
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Patrocinadores.Include(p => p.IdHeroeNavigation);
            return View(await appDbContext.ToListAsync());
        }
        [HttpGet("patrocinador")]
        public async Task<ActionResult<IEnumerable<Patrocinador>>> GetAlumnos()
        {
            return await _context.Patrocinadores.ToListAsync();
        }
        // GET: Patrocinadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patrocinadores == null)
            {
                return NotFound();
            }

            var patrocinador = await _context.Patrocinadores
                .Include(p => p.IdHeroeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patrocinador == null)
            {
                return NotFound();
            }

            return View(patrocinador);
        }

        // GET: Patrocinadores/Create
        public IActionResult Create()
        {
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id");
            return View();
        }

        // POST: Patrocinadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdHeroe,Nombre,Monto,OrigenDinero")] Patrocinador patrocinador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patrocinador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id", patrocinador.IdHeroe);
            return View(patrocinador);
        }

        // GET: Patrocinadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patrocinadores == null)
            {
                return NotFound();
            }

            var patrocinador = await _context.Patrocinadores.FindAsync(id);
            if (patrocinador == null)
            {
                return NotFound();
            }
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id", patrocinador.IdHeroe);
            return View(patrocinador);
        }

        // POST: Patrocinadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdHeroe,Nombre,Monto,OrigenDinero")] Patrocinador patrocinador)
        {
            if (id != patrocinador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patrocinador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatrocinadorExists(patrocinador.Id))
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
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id", patrocinador.IdHeroe);
            return View(patrocinador);
        }

        // GET: Patrocinadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patrocinadores == null)
            {
                return NotFound();
            }

            var patrocinador = await _context.Patrocinadores
                .Include(p => p.IdHeroeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patrocinador == null)
            {
                return NotFound();
            }

            return View(patrocinador);
        }

        // POST: Patrocinadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Patrocinadores == null)
            {
                return Problem("Entity set 'AppDbContext.Patrocinadores'  is null.");
            }
            var patrocinador = await _context.Patrocinadores.FindAsync(id);
            if (patrocinador != null)
            {
                _context.Patrocinadores.Remove(patrocinador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpGet("LosPatrocinadores")]
        public async Task<IActionResult> ObtenerPatrocinadores()
        {
            var patrocinadores = await _context.Patrocinadores.ToListAsync();

            return Ok(patrocinadores);
        }

        [HttpPost("CrearPatrocinador")]
        public async Task<IActionResult> CrearPatrocinador([FromBody] Patrocinador patrocinadorModel)
        {
 

            string sql = $"EXEC InsertarNuevoPatrocinador " +
                         $"@id_heroe = {patrocinadorModel.IdHeroe}, " +
                         $"@nombre = '{patrocinadorModel.Nombre}', " +
                         $"@monto = {patrocinadorModel.Monto}, " +
                         $"@origen_dinero = '{patrocinadorModel.OrigenDinero}'";

            await _context.Database.ExecuteSqlRawAsync(sql);

            return Ok("Patrocinador creado exitosamente");
        }



        [HttpDelete("BorrarPatrocinador/{id}")]
        public async Task<IActionResult> BorrarPatrocinador(int id)
        {
            var patrocinador = await _context.Patrocinadores.FindAsync(id);

            if (patrocinador == null)
            {
                return NotFound();
            }

            _context.Patrocinadores.Remove(patrocinador);
            await _context.SaveChangesAsync();

            return Ok("Patrocinador borrado exitosamente");
        }

        [HttpPut("ActualizarPatrocinador/{id}")]
        public async Task<IActionResult> ActualizarPatrocinador(int id, [FromBody] Patrocinador patrocinadorModel)
        {


            var patrocinador = await _context.Patrocinadores.FindAsync(id);

            if (patrocinador == null)
            {
                return NotFound();
            }

            patrocinador.Nombre = patrocinadorModel.Nombre;
            patrocinador.Monto = patrocinadorModel.Monto;
            patrocinador.OrigenDinero = patrocinadorModel.OrigenDinero;

            _context.Patrocinadores.Update(patrocinador);
            await _context.SaveChangesAsync();

            return Ok("Patrocinador actualizado exitosamente");
        }


        [HttpGet("PatrocinadorConMayorMonto/{idHeroe}")]
        public async Task<IActionResult> ObtenerPatrocinadorConMayorMonto(int idHeroe)
        {
            var patrocinador = await _context.Patrocinadores
                
                .Where(p => p.IdHeroe == idHeroe)
                .OrderByDescending(p => p.Monto)
                .FirstOrDefaultAsync();

            if (patrocinador == null)
            {
                return NotFound("No se encontró un patrocinador para el héroe especificado.");
            }

            return Ok(patrocinador);
        }

        private bool PatrocinadorExists(int id)
        {
          return (_context.Patrocinadores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
