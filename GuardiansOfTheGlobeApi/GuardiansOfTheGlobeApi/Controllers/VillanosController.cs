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

        [HttpGet("villanos/prodalma")]
        public async Task<IActionResult> GetVillanosMas()
        {
            var villanoList = await _context.Villanos
                .FromSqlRaw("Exec ObtenerVillanoConMasPeleas")
                .ToListAsync();

            return Ok(villanoList);
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

        private bool VillanoExists(int id)
        {
          return (_context.Villanos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
