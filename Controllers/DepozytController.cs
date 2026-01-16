using DepozytOpon.Data;
using DepozytOpon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DepozytOpon.Controllers
{
    [Authorize]
    public class DepozytController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepozytController(ApplicationDbContext context)
        {
            _context = context;

            // 🔐 WYMAGANE przez QuestPDF
            QuestPDF.Settings.License = LicenseType.Community;
        }

        // LISTA
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Depozyt.ToListAsync();
            return View(lista);
        }

        // DODAWANIE
        public IActionResult Dodaj()
        {
            ViewBag.Opony = new SelectList(_context.Opony, "KodTowaru", "KodTowaru");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dodaj(Depozyt depozyt)
        {
            // WALIDACJA UNIKALNOŚCI NumerBOX
            if (_context.Depozyt.Any(d => d.NumerBOX == depozyt.NumerBOX))
            {
                ModelState.AddModelError(
                    nameof(depozyt.NumerBOX),
                    "Depozyt o podanym numerze BOX już istnieje"
                );
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Opony = new SelectList(
                    _context.Opony,
                    "KodTowaru",
                    "KodTowaru",
                    depozyt.OponaId
                );
                return View(depozyt);
            }

            depozyt.DataPrzyjecia = DateTime.Now;
            depozyt.UtworzonoPrzez = User.Identity.Name;

            _context.Depozyt.Add(depozyt);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Depozyt/Edytuj/5
        public async Task<IActionResult> Edytuj(int id)
        {
            var depozyt = await _context.Depozyt.FindAsync(id);
            if (depozyt == null)
                return NotFound();

            ViewBag.Opony = new SelectList(_context.Opony, "KodTowaru", "KodTowaru", depozyt.OponaId);
            return View(depozyt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edytuj(int id, Depozyt depozytEdytowany)
        {
            var depozyt = await _context.Depozyt.FindAsync(id);
            if (depozyt == null)
                return NotFound();

            // 🔎 WALIDACJA UNIKALNOŚCI NumerBOX (z pominięciem bieżącego rekordu)
            bool numerBoxIstnieje = await _context.Depozyt
                .AnyAsync(d => d.NumerBOX == depozytEdytowany.NumerBOX && d.Id != id);

            if (numerBoxIstnieje)
            {
                ModelState.AddModelError(
                    nameof(depozytEdytowany.NumerBOX),
                    "Depozyt o podanym numerze BOX już istnieje"
                );
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Opony = new SelectList(
                    _context.Opony,
                    "KodTowaru",
                    "KodTowaru",
                    depozytEdytowany.OponaId
                );

                return View(depozytEdytowany);
            }

            // ✏️ Kopiujemy tylko dozwolone pola
            depozyt.NumerBOX = depozytEdytowany.NumerBOX;
            depozyt.ImieNazwisko = depozytEdytowany.ImieNazwisko;
            depozyt.NumerTelefonu = depozytEdytowany.NumerTelefonu;
            depozyt.MarkaPojazdu = depozytEdytowany.MarkaPojazdu;
            depozyt.RejestracjaPojazdu = depozytEdytowany.RejestracjaPojazdu;
            depozyt.OponaId = depozytEdytowany.OponaId;
            depozyt.Ilosc = depozytEdytowany.Ilosc;
            depozyt.Notatka = depozytEdytowany.Notatka;

            // 🧾 Audyt
            depozyt.EdytowanoPrzez = User.Identity?.Name;
            depozyt.DataEdycji = DateTime.Now;

            await _context.SaveChangesAsync();

            // ✅ MODAL SUKCESU
            TempData["ModalMessage"] = "Depozyt został pomyślnie edytowany.";
            TempData["ModalType"] = "success";

            return RedirectToAction(nameof(Index));
        }



        // USUWANIE
        public async Task<IActionResult> Usun(int id)
        {
            var depozyt = await _context.Depozyt.FindAsync(id);
            if (depozyt == null)
                return NotFound();

            return View(depozyt);
        }

        [HttpPost, ActionName("UsunPotwierdzenie")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunPotwierdzony(int id)
        {
            var depozyt = await _context.Depozyt.FindAsync(id);
            if (depozyt != null)
            {
                _context.Depozyt.Remove(depozyt);
                await _context.SaveChangesAsync();

                TempData["ModalMessage"] = "Depozyt został usunięty.";
                TempData["ModalType"] = "danger";
            }

            return RedirectToAction(nameof(Index));

        }

        // WSPÓLNY PAYLOAD QR
        private static string BuildQrPayload(Depozyt d)
        {
            return
        $@"BOX:{d.NumerBOX}
        POJAZD:{d.MarkaPojazdu}
        REJ:{d.RejestracjaPojazdu}
        DATA:{d.DataPrzyjecia:yyyy-MM-dd}";
        }


        private static byte[] GenerateQrBytes(string payload)
        {
            using var gen = new QRCodeGenerator();
            using var data = gen.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            var qr = new PngByteQRCode(data);
            return qr.GetGraphic(20);
        }

        // QR NA LIŚCIE (FE)
        public IActionResult GenerateQR(int id)
        {
            var depozyt = _context.Depozyt.FirstOrDefault(d => d.Id == id);
            if (depozyt == null)
                return NotFound();

            var payload = BuildQrPayload(depozyt);
            var qrBytes = GenerateQrBytes(payload);

            return File(qrBytes, "image/png");
        }

        // PODSUMOWANIE PDF
        public IActionResult Summary(int id)
        {
            var depozyt = _context.Depozyt.FirstOrDefault(d => d.Id == id);
            if (depozyt == null)
                return NotFound();

            var payload = BuildQrPayload(depozyt);
            var qrBytes = GenerateQrBytes(payload);

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Text("PODSUMOWANIE DEPOZYTU")
                            .FontSize(20)
                            .Bold();

                        col.Item().LineHorizontal(1);

                        col.Item().Text($"Numer BOX: {depozyt.NumerBOX}");
                        col.Item().Text($"Klient: {depozyt.ImieNazwisko}");
                        col.Item().Text($"Telefon: {depozyt.NumerTelefonu}");
                        col.Item().Text($"Pojazd: {depozyt.MarkaPojazdu}");
                        col.Item().Text($"Rejestracja: {depozyt.RejestracjaPojazdu}");
                        col.Item().Text($"Kod opony: {depozyt.OponaId}");
                        col.Item().Text($"Ilość: {depozyt.Ilosc}");
                        col.Item().Text($"Data przyjęcia: {depozyt.DataPrzyjecia:yyyy-MM-dd}");

                        col.Item().PaddingTop(20);

                        col.Item().AlignCenter()
                        .Image(qrBytes, ImageScaling.FitWidth);

                        col.Item().AlignCenter()
                            .Text("Kod QR depozytu")
                            .Italic()
                            .FontSize(10);
                    });
                });
            }).GeneratePdf();

            return File(
                pdf,
                "application/pdf",
                $"Depozyt_BOX_{depozyt.NumerBOX}.pdf"
            );
        }

        [HttpPost]
        public IActionResult QRSearch(string qr)
        {
            if (string.IsNullOrWhiteSpace(qr))
                return BadRequest();

            // rozbijamy linie QR
            var lines = qr
                .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .ToList();

            // szukamy linii BOX:
            var boxLine = lines.FirstOrDefault(l => l.StartsWith("BOX:"));
            if (boxLine == null)
                return BadRequest("Brak numeru BOX w QR");

            var boxNumber = boxLine.Replace("BOX:", "").Trim();

            // szukamy depozytu po BOX
            var depozyt = _context.Depozyt
                .FirstOrDefault(d => d.NumerBOX == boxNumber);

            if (depozyt == null)
                return NotFound("Nie odnaleziono depozytu");

            return RedirectToAction("Edytuj", new { id = depozyt.Id });
        }

    }
}
