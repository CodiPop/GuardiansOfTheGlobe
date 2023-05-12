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
    public class PeleasController : Controller
    {
        private readonly AppDbContext _context;

        public PeleasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Peleas
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Peleas.Include(p => p.IdHeroeNavigation).Include(p => p.IdVillanoNavigation);
            return View(await appDbContext.ToListAsync());
        }
        [HttpGet("pelea")]
        public async Task<ActionResult<IEnumerable<Pelea>>> GetAlumnos()
        {
            return await _context.Peleas.ToListAsync();
        }
        // GET: Peleas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Peleas == null)
            {
                return NotFound();
            }

            var pelea = await _context.Peleas
                .Include(p => p.IdHeroeNavigation)
                .Include(p => p.IdVillanoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelea == null)
            {
                return NotFound();
            }

            return View(pelea);
        }

        // GET: Peleas/Create
        public IActionResult Create()
        {
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id");
            ViewData["IdVillano"] = new SelectList(_context.Villanos, "Id", "Id");
            return View();
        }

        // POST: Peleas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdHeroe,IdVillano,Resultado")] Pelea pelea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pelea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id", pelea.IdHeroe);
            ViewData["IdVillano"] = new SelectList(_context.Villanos, "Id", "Id", pelea.IdVillano);
            return View(pelea);
        }

        // GET: Peleas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Peleas == null)
            {
                return NotFound();
            }

            var pelea = await _context.Peleas.FindAsync(id);
            if (pelea == null)
            {
                return NotFound();
            }
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id", pelea.IdHeroe);
            ViewData["IdVillano"] = new SelectList(_context.Villanos, "Id", "Id", pelea.IdVillano);
            return View(pelea);
        }

        // POST: Peleas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdHeroe,IdVillano,Resultado")] Pelea pelea)
        {
            if (id != pelea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pelea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeleaExists(pelea.Id))
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
            ViewData["IdHeroe"] = new SelectList(_context.Heroes, "Id", "Id", pelea.IdHeroe);
            ViewData["IdVillano"] = new SelectList(_context.Villanos, "Id", "Id", pelea.IdVillano);
            return View(pelea);
        }

        // GET: Peleas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Peleas == null)
            {
                return NotFound();
            }

            var pelea = await _context.Peleas
                .Include(p => p.IdHeroeNavigation)
                .Include(p => p.IdVillanoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelea == null)
            {
                return NotFound();
            }

            return View(pelea);
        }

        // POST: Peleas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Peleas == null)
            {
                return Problem("Entity set 'AppDbContext.Peleas'  is null.");
            }
            var pelea = await _context.Peleas.FindAsync(id);
            if (pelea != null)
            {
                _context.Peleas.Remove(pelea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeleaExists(int id)
        {
          return (_context.Peleas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
