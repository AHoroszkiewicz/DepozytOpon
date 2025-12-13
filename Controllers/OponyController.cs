using DepozytOpon.Data;
using DepozytOpon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

namespace DepozytOpon.Controllers
{
    [Authorize]
    public class OponyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OponyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTA
        public async Task<IActionResult> Index()
        {
            var opony = await _context.Opony.ToListAsync();
            return View(opony);
        }

        // DODAWANIE — GET
        public IActionResult Dodaj()
        {
            return View();
        }

        // DODAWANIE — POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(Opona opona)
        {
            if (ModelState.IsValid)
            {
                _context.Opony.Add(opona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(opona);
        }

        // EDYCJA — GET
        public async Task<IActionResult> Edytuj(int id)
        {
            var opona = await _context.Opony.FindAsync(id);
            if (opona == null)
                return NotFound();

            return View(opona);
        }

        // EDYCJA — POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(Opona opona)
        {
            if (ModelState.IsValid)
            {
                _context.Update(opona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(opona);
        }

        // USUWANIE — GET (potwierdzenie)
        public async Task<IActionResult> Usun(int id)
        {
            var opona = await _context.Opony.FindAsync(id);
            if (opona == null)
                return NotFound();

            return View(opona);
        }

        // USUWANIE — POST
        [HttpPost, ActionName("UsunPotwierdzenie")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzony(int id)
        {
            var opona = await _context.Opony.FindAsync(id);
            if (opona != null)
            {
                _context.Opony.Remove(opona);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
