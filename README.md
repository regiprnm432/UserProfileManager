# UserProfileManager

Aplikasi manajemen profil pengguna berbasis **ASP.NET Core MVC** dengan fitur upload foto, validasi form, dan export data ke PDF menggunakan **QuestPDF**.

## Fitur

- Input data pengguna lengkap (identitas, alamat, latar belakang).
- Upload foto dengan validasi format dan ukuran (JPG/PNG, maksimal 2MB).
- Validasi pilihan Gender dan Status Perkawinan.
- Menyimpan data ke database menggunakan **Entity Framework Core**.
- Menampilkan daftar pengguna yang tersimpan di database.
- Export data pengguna ke PDF.

## Teknologi yang Digunakan

- **.NET 8**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **QuestPDF**
- **Bootstrap 5** untuk tampilan UI
- **SQLite** (default) / bisa diganti sesuai kebutuhan

## Persyaratan

Sebelum menjalankan proyek ini, pastikan sudah terpasang:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQLite](https://www.sqlite.org/download.html) atau database lain yang Anda konfigurasi
- Git (opsional)

## Cara Menjalankan Proyek

1. **Clone repository**

   ```bash
   git clone https://github.com/username/UserProfileManager.git
   cd UserProfileManager
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Apply migrations dan update database**

   ```bash
   dotnet ef database update
   ```

4. **Jalankan aplikasi**

   ```bash
   dotnet run
   ```

5. **Akses di browser**
   Buka: [http://localhost:5080](http://localhost:5080)

## Struktur Proyek

```
UserProfileManager/
├── Controllers/         # Controller MVC (UsersController.cs)
├── Data/                # DbContext dan konfigurasi database
├── Models/              # Model entity
├── ViewModels/          # View model untuk form input
├── Views/               # File Razor view (.cshtml)
│   ├── Shared/          # Layout dan partial view
│   └── Users/           # View khusus untuk pengguna
├── wwwroot/             # Asset statis (CSS, JS, uploads)
├── appsettings.json     # Konfigurasi aplikasi
└── Program.cs           # Entry point aplikasi
```

## Catatan

- Direktori `wwwroot/uploads` akan otomatis dibuat ketika upload foto pertama kali.
- Pastikan folder `wwwroot/uploads` memiliki permission tulis (write permission).

## Lisensi

Proyek ini menggunakan lisensi **MIT**.
