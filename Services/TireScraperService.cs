using Microsoft.Playwright;

namespace DepozytOpon.Services
{
    public class TireScraperService
    {
        public async Task<ScrapedTireDto> ScrapeAsync(string url)
        {
            var dto = new ScrapedTireDto();

            try
            {
                using var playwright = await Playwright.CreateAsync();
                await using var browser = await playwright.Chromium.LaunchAsync(
                    new BrowserTypeLaunchOptions { Headless = true });

                var page = await browser.NewPageAsync();
                await page.GotoAsync(url, new PageGotoOptions { Timeout = 15000 });

                dto.Typ = await SafeText(page.Locator("div.item:has(div.label:text('Typ')) div.content"), "Typ");
                dto.Producent = await SafeText(page.Locator("div.name .producer"), "Producent");
                dto.Rozmiar = await SafeText(page.Locator("div.item:has(div.label:text('Rozmiar')) div.content"), "Rozmiar");
                dto.Bieznik = await SafeText(page.Locator("div.item:has(div.label:text('Bieżnik')) div.content"), "Bieżnik");
                dto.Sezon = await SafeText(page.Locator("div.item:has(div.label:text('Sezon')) div.content"), "Sezon");
                dto.IndeksPredkosci = await SafeText(page.Locator("div.item:has(div.label:text('Indeks Prędkości')) div.content"), "Indeks prędkości");
                dto.IndeksNososci = await SafeText(page.Locator("div.item:has(div.label:text('Indeks nośności')) div.content"), "Indeks nośności");
                dto.RokProdukcji = await SafeText(page.Locator("div.item:has(div.label:text('Rok produkcji')) div.content"), "Rok produkcji");
                dto.KodTowaru = await SafeText(page.Locator("div.item:has(div.label:text('Kod towaru')) div.content"), "Kod towaru");
                dto.NowaEtykietaUE = await SafeText(page.Locator("div.item:has(div.label:text('Nowa etykieta UE')) div.content"), "Nowa etykieta UE");
                dto.EPREL = await SafeText(page.Locator("div.item:has(div.label:text('EPREL')) div.content"), "EPREL");
                dto.Gwarancja = await SafeText(page.Locator("div.item:has(div.label:text('Gwarancja')) div.content"), "Gwarancja");
                dto.CzasDostawy = await SafeText(page.Locator("div.item:has(div.label:text('Czas dostawy')) div.content"), "Czas dostawy");

                dto.Success = true;
                return dto;
            }
            catch (Exception ex)
            {
                dto.Success = false;
                dto.Error = ex.Message;
                return dto;
            }
        }

        private async Task<string?> SafeText(ILocator locator, string fieldName)
        {
            try
            {
                if (locator == null || await locator.CountAsync() == 0)
                    return $"(nie znaleziono wartości dla pola: {fieldName})";

                var text = (await locator.First.InnerTextAsync())?.Trim();

                if (string.IsNullOrWhiteSpace(text))
                    return $"(nie znaleziono wartości dla pola: {fieldName})";

                return text;
            }
            catch
            {
                return $"(nie znaleziono wartości dla pola: {fieldName})";
            }
        }

    }

    public class ScrapedTireDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }

        public string? Typ { get; set; }
        public string? Producent { get; set; }
        public string? Rozmiar { get; set; }
        public string? Bieznik { get; set; }
        public string? Sezon { get; set; }
        public string? IndeksPredkosci { get; set; }
        public string? IndeksNososci { get; set; }
        public string? RokProdukcji { get; set; }
        public string? KodTowaru { get; set; }
        public string? NowaEtykietaUE { get; set; }
        public string? EPREL { get; set; }
        public string? Gwarancja { get; set; }
        public string? CzasDostawy { get; set; }
    }
}
