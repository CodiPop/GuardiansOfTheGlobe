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

        private bool PatrocinadorExists(int id)
        {
          return (_context.Patrocinadores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
