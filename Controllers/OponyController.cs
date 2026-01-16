using DepozytOpon.Data;
using DepozytOpon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        // =======================
        // DODAWANIE
        // =======================

        // GET
        public IActionResult Dodaj()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(Opona opona)
        {
            // 🔎 walidacja unikalności KodTowaru
            bool kodIstnieje = await _context.Opony
                .AnyAsync(o => o.KodTowaru == opona.KodTowaru);

            if (kodIstnieje)
            {
                ModelState.AddModelError(
                    nameof(opona.KodTowaru),
                    "Opona o podanym kodzie towaru już istnieje."
                );
            }

            if (!ModelState.IsValid)
                return View(opona);

            _context.Opony.Add(opona);
            await _context.SaveChangesAsync();

            TempData["ModalMessage"] = "Opona została pomyślnie dodana.";
            TempData["ModalType"] = "success";

            return RedirectToAction(nameof(Index));
        }

        // =======================
        // EDYCJA
        // =======================

        // GET
        public async Task<IActionResult> Edytuj(int id)
        {
            var opona = await _context.Opony.FindAsync(id);
            if (opona == null)
                return NotFound();

            return View(opona);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(Opona opona)
        {
            // 🔎 walidacja unikalności KodTowaru (pomijamy edytowany rekord)
            bool kodIstnieje = await _context.Opony
                .AnyAsync(o => o.KodTowaru == opona.KodTowaru && o.Id != opona.Id);

            if (kodIstnieje)
            {
                ModelState.AddModelError(
                    nameof(opona.KodTowaru),
                    "Inna opona posiada już ten kod towaru."
                );
            }

            if (!ModelState.IsValid)
                return View(opona);

            _context.Update(opona);
            await _context.SaveChangesAsync();

            TempData["ModalMessage"] = "Opona została zaktualizowana.";
            TempData["ModalType"] = "success";

            return RedirectToAction(nameof(Index));
        }

        // =======================
        // USUWANIE
        // =======================

        // GET (potwierdzenie)
        public async Task<IActionResult> Usun(int id)
        {
            var opona = await _context.Opony.FindAsync(id);
            if (opona == null)
                return NotFound();

            return View(opona);
        }

        // POST
        [HttpPost, ActionName("UsunPotwierdzenie")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzony(int id)
        {
            var opona = await _context.Opony.FindAsync(id);
            if (opona != null)
            {
                _context.Opony.Remove(opona);
                await _context.SaveChangesAsync();

                TempData["ModalMessage"] = "Opona została usunięta.";
                TempData["ModalType"] = "success";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
