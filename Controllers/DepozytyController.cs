using DepozytOpon.Data;
using DepozytOpon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
// --- NOWE BIBLIOTEKI DO QR ---
using QRCoder;
using System.IO;
// -----------------------------

namespace DepozytOpon.Controllers
{
    [Authorize]
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
        public IActionResult Dodaj()
        {
            ViewBag.Opony = new SelectList(_context.Opony, "KodTowaru", "KodTowaru");
            return View();
        }

        // DODAWANIE — POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(Depozyt depozyt)
        {
            if (ModelState.IsValid)
            {
                depozyt.DataPrzyjecia = DateTime.Now;
                _context.Depozyty.Add(depozyt);
                await _context.SaveChangesAsync();

                // ZMIANA: Po dodaniu przekieruj do szczegółów, żeby pokazać QR
                // return RedirectToAction(nameof(Index)); 
                // Możesz przekierować do akcji np. "Szczegoly", jeśli ją stworzysz, 
                // ale na razie zostawmy Index, a QR dodasz na liście lub w edycji.
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Opony = new SelectList(_context.Opony, "KodTowaru", "KodTowaru", depozyt.OponaId);
            return View(depozyt);
        }

        // EDYCJA — GET
        public async Task<IActionResult> Edytuj(int id)
        {
            var depozyt = await _context.Depozyty.FindAsync(id);
            if (depozyt == null)
                return NotFound();

            ViewBag.Opony = new SelectList(_context.Opony, "KodTowaru", "KodTowaru", depozyt.OponaId);
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

            ViewBag.Opony = new SelectList(_context.Opony, "KodTowaru", "KodTowaru", depozyt.OponaId);
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

        // --- NOWA METODA: GENEROWANIE KODU QR ---
        // Wywołujesz ją w HTML jako obrazek
        public IActionResult GenerujQR(int id)
        {
            // Możemy zakodować samo ID, np. "15", albo prefiks np. "DEP-15"
            string payload = id.ToString();

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

                // PngByteQRCode jest lżejsze i nie wymaga System.Drawing.Common w nowszych .NET
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeImage = qrCode.GetGraphic(20);

                return File(qrCodeImage, "image/png");
            }
        }
    }
}
