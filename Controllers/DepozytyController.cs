using DepozytOpon.Data;
using DepozytOpon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace DepozytOpon.Controllers
{
    public class DepozytyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepozytyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTA
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Depozyty.ToListAsync();
            return View(lista);
        }

        // DODAWANIE — GET
        // GET
        public IActionResult Dodaj()
        {
            ViewBag.Opony = new SelectList(_context.Opony, "Id", "KodTowaru");
            return View();
        }

        // DODAWANIE — POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(Depozyt depozyt)
        {
            if (ModelState.IsValid)
            {
                // przypisanie daty
                depozyt.DataPrzyjecia = DateTime.Now;

                //// Pobranie obiektu Opona z bazy
                //var opona = await _context.Opony.FindAsync(depozyt.OponaId);
                //if (opona == null)
                //{
                //    ModelState.AddModelError("OponaId", "Wybrana opona nie istnieje.");
                //    ViewBag.Opony = new SelectList(_context.Opony, "Id", "KodTowaru");
                //    return View(depozyt);
                //}

                //depozyt.Opona = opona;

                // Dodanie depozytu
                _context.Depozyty.Add(depozyt);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // jeśli ModelState nie jest valid
            ViewBag.Opony = new SelectList(_context.Opony, "Id", "KodTowaru", depozyt.OponaId);
            return View(depozyt);
        }


        // EDYCJA — GET
        public async Task<IActionResult> Edytuj(int id)
        {
            var depozyt = await _context.Depozyty.FindAsync(id);
            if (depozyt == null)
                return NotFound();

            return View(depozyt);
        }

        // EDYCJA — POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(Depozyt depozyt)
        {
            if (ModelState.IsValid)
            {
                _context.Update(depozyt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(depozyt);
        }

        // USUWANIE — GET
        public async Task<IActionResult> Usun(int id)
        {
            var depozyt = await _context.Depozyty.FindAsync(id);
            if (depozyt == null)
                return NotFound();

            return View(depozyt);
        }

        // USUWANIE — POST
        [HttpPost, ActionName("UsunPotwierdzenie")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzony(int id)
        {
            var depozyt = await _context.Depozyty.FindAsync(id);
            if (depozyt != null)
            {
                _context.Depozyty.Remove(depozyt);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
