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
    public class AgendasController : Controller
    {
        private readonly AppDbContext _context;

        public AgendasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Agendas
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Agenda.Include(a => a.IdHeroeNavigation);
            return View(await appDbContext.ToListAsync());
        }
        [HttpGet("agenda")]
        public async Task<ActionResult<IEnumerable<Agenda>>> GetAlumnos()
        {
            return await _context.Agenda.ToListAsync();
        }

        // GET: Agendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Agenda == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agenda
                .Include(a => a.IdHeroeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        // GET: Agendas/Create
        public IActionResult Create()
        {
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id");
            return View();
        }

        // POST: Agendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdHeroe,Fecha,Evento")] Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agenda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id", agenda.IdHeroe);
            return View(agenda);
        }

        // GET: Agendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Agenda == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agenda.FindAsync(id);
            if (agenda == null)
            {
                return NotFound();
            }
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id", agenda.IdHeroe);
            return View(agenda);
        }

        // POST: Agendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdHeroe,Fecha,Evento")] Agenda agenda)
        {
            if (id != agenda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendaExists(agenda.Id))
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
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id", agenda.IdHeroe);
            return View(agenda);
        }

        // GET: Agendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Agenda == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agenda
                .Include(a => a.IdHeroeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        // POST: Agendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Agenda == null)
            {
                return Problem("Entity set 'AppDbContext.Agenda'  is null.");
            }
            var agenda = await _context.Agenda.FindAsync(id);
            if (agenda != null)
            {
                _context.Agenda.Remove(agenda);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpPost("CrearEvento")]
        public async Task<IActionResult> CrearEvento([FromBody] Agenda eventoModel)
        {
 

            var query = $"EXEC InsertarNuevoEventoEnAgenda @id_heroe = {eventoModel.IdHeroe}, @fecha = '{eventoModel.Fecha}', @evento = '{eventoModel.Evento}'";

            await _context.Database.ExecuteSqlRawAsync(query);

            return Ok("Evento creado exitosamente");
        }


        [HttpPut("ActualizarEvento/{id}")]
        public async Task<IActionResult> ActualizarEvento(int id, [FromBody] Agenda eventoModel)
        {
          

            var evento = await _context.Agenda.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            evento.IdHeroe = eventoModel.IdHeroe;
            evento.Fecha = eventoModel.Fecha;
            evento.Evento = eventoModel.Evento;

            _context.Agenda.Update(evento);
            await _context.SaveChangesAsync();

            return Ok("Evento actualizado exitosamente");
        }

        [HttpDelete("EliminarEvento/{id}")]
        public async Task<IActionResult> EliminarEvento(int id)
        {
            var evento = await _context.Agenda.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            _context.Agenda.Remove(evento);
            await _context.SaveChangesAsync();

            return Ok("Evento eliminado exitosamente");
        }
        private bool AgendaExists(int id)
        {
          return (_context.Agenda?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
