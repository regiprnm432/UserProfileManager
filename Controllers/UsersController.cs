using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProfileManager.Data;
using UserProfileManager.Models;
using UserProfileManager.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

namespace UserProfileManager.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        // Opsi yang diperbolehkan
        private static readonly string[] AllowedGenders = new[] { "Laki-laki", "Perempuan" };
        private static readonly string[] AllowedMaritalStatuses = new[] { "Lajang", "Menikah" };
        private static readonly string[] AllowedImageExts = new[] { ".jpg", ".jpeg", ".png" };

        public UsersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // GET: /Users/Create
        public IActionResult Create() => View(new UserCreateViewModel());

        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel vm)
        {
            // Validasi khusus Gender & MaritalStatus (wajib pilih & harus valid)
            var gender = vm.Gender?.Trim();
            if (string.IsNullOrWhiteSpace(gender) || !AllowedGenders.Contains(gender))
                ModelState.AddModelError(nameof(vm.Gender), "Pilih gender (Laki-laki atau Perempuan).");

            var marital = vm.MaritalStatus?.Trim();
            if (string.IsNullOrWhiteSpace(marital) || !AllowedMaritalStatuses.Contains(marital))
                ModelState.AddModelError(nameof(vm.MaritalStatus), "Pilih status pernikahan (Lajang atau Menikah).");

            // Foto wajib diunggah
            if (vm.Photo is null || vm.Photo.Length == 0)
            {
                ModelState.AddModelError(nameof(vm.Photo), "Foto wajib diunggah.");
            }
            else
            {
                // Validasi format & ukuran foto
                var ext = Path.GetExtension(vm.Photo.FileName).ToLowerInvariant();
                if (!AllowedImageExts.Contains(ext))
                    ModelState.AddModelError(nameof(vm.Photo), "Format file harus JPG/PNG.");
                // 2 MB batas contoh
                if (vm.Photo.Length > 2 * 1024 * 1024)
                    ModelState.AddModelError(nameof(vm.Photo), "Ukuran maksimal 2MB.");
            }

            if (!ModelState.IsValid) return View(vm);

            // Simpan foto
            string? savedFileName = null;
            var fileExt = Path.GetExtension(vm.Photo!.FileName).ToLowerInvariant();
            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var uploadsDir = Path.Combine(webRoot, "uploads");
            Directory.CreateDirectory(uploadsDir);

            savedFileName = $"{Guid.NewGuid():N}{fileExt}";
            var fullPath = Path.Combine(uploadsDir, savedFileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await vm.Photo.CopyToAsync(stream);
            }

            var user = new User
            {
                FullName = vm.FullName,
                NIK = vm.NIK,
                Email = vm.Email,
                Gender = gender,
                BirthDate = vm.BirthDate,
                BirthPlace = vm.BirthPlace?.Trim(),
                Phone = vm.Phone?.Trim(),
                Address = vm.Address,
                City = vm.City?.Trim(),
                Province = vm.Province?.Trim(),
                PostalCode = vm.PostalCode?.Trim(),
                Nationality = vm.Nationality?.Trim(),
                MaritalStatus = marital,
                Occupation = vm.Occupation?.Trim(),
                EducationLevel = vm.EducationLevel?.Trim(),
                BloodType = vm.BloodType?.Trim(),
                PhotoFileName = savedFileName
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = user.Id });
        }

        // GET: /Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user is null) return NotFound();
            return View(user);
        }

        // GET: /Users/ExportPdf/5
        public async Task<IActionResult> ExportPdf(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user is null) return NotFound();

            QuestPDF.Settings.License = LicenseType.Community;

            // Siapkan path foto jika ada
            string? photoPath = null;
            if (!string.IsNullOrEmpty(user.PhotoFileName))
            {
                var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                var uploadsDir = Path.Combine(webRoot, "uploads");
                var full = Path.Combine(uploadsDir, user.PhotoFileName);
                if (System.IO.File.Exists(full))
                    photoPath = full;
            }

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);
                    page.DefaultTextStyle(t => t.FontSize(11));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Data User").Bold().FontSize(18);
                            col.Item().Text($"Dibuat: {user.CreatedAt:dd MMM yyyy HH:mm} UTC");
                        });

                        row.ConstantItem(100).Height(100).AlignRight().AlignMiddle().Element(e =>
                        {
                            if (photoPath is not null)
                            {
                                var image = Image.FromFile(photoPath);
                                e.Image(image).FitArea();
                            }
                        });
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().Text("Identitas").Bold().FontSize(14);

                        col.Item().Table(t =>
                        {
                            t.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(1);
                                c.RelativeColumn(2);
                            });

                            static IContainer CellLabel(IContainer c) =>
                                c.BorderBottom(1).PaddingVertical(3).PaddingRight(8);

                            static IContainer CellValue(IContainer c) =>
                                c.BorderBottom(1).PaddingVertical(3);

                            void Row(string label, string? value)
                            {
                                t.Cell().Element(CellLabel).Text(x => x.Span(label).SemiBold());
                                t.Cell().Element(CellValue).Text(value ?? "-");
                            }

                            Row("Nama Lengkap", user.FullName);
                            Row("NIK", user.NIK);
                            Row("Email", user.Email);
                            Row("Telepon", user.Phone);
                            Row("Jenis Kelamin", user.Gender);
                            Row("Tempat/Tgl Lahir", $"{user.BirthPlace ?? "-"}, {user.BirthDate:dd MMM yyyy}");
                            Row("Alamat", user.Address);
                            Row("Kota", user.City);
                            Row("Provinsi", user.Province);
                            Row("Kode Pos", user.PostalCode);
                            Row("Kewarganegaraan", user.Nationality);
                            Row("Status Pernikahan", user.MaritalStatus);
                            Row("Pekerjaan", user.Occupation);
                            Row("Pendidikan Terakhir", user.EducationLevel);
                            Row("Golongan Darah", user.BloodType);
                        });
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Generated by ASP.NET Core + QuestPDF • ");
                        txt.Span($"{DateTime.UtcNow:dd MMM yyyy HH:mm} UTC");
                    });
                });
            }).GeneratePdf();

            var fileName = $"User-{user.Id}-{SanitizeFileName(user.FullName)}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        // GET: /Users
        [HttpGet]
        public async Task<IActionResult> Index(string? q)
        {
            var query = _db.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(u =>
                    EF.Functions.Like(u.FullName!, $"%{term}%") ||
                    EF.Functions.Like(u.NIK!, $"%{term}%") ||
                    EF.Functions.Like(u.Email!, $"%{term}%"));
            }

            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            ViewData["q"] = q;
            return View(users);
        }

        private static string SanitizeFileName(string input)
        {
            var invalid = Path.GetInvalidFileNameChars();
            return new string(input.Where(c => !invalid.Contains(c)).ToArray());
        }
    }
}
