using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace UserProfileManager.ViewModels;

public class UserCreateViewModel
{
    [Display(Name = "Nama Lengkap")]
    [Required]
    public string FullName { get; set; } = default!;

    [Display(Name = "NIK")]
    [Required, StringLength(32)]
    public string NIK { get; set; } = default!;

    [Display(Name = "Email")]
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Display(Name = "Nomor Telepon")]
    public string? Phone { get; set; }

    [Display(Name = "Jenis Kelamin")]
    [Required(ErrorMessage = "Pilih gender")]
    public string? Gender { get; set; }   // sengaja nullable agar default kosong validasi

    [Display(Name = "Tempat Lahir")]
    public string? BirthPlace { get; set; }

    [Display(Name = "Tanggal Lahir")]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Display(Name = "Alamat")]
    public string? Address { get; set; }

    [Display(Name = "Kota")]
    public string? City { get; set; }

    [Display(Name = "Provinsi")]
    public string? Province { get; set; }

    [Display(Name = "Kode Pos")]
    public string? PostalCode { get; set; }

    [Display(Name = "Kewarganegaraan")]
    public string? Nationality { get; set; }

    [Display(Name = "Status Pernikahan")]
    [Required(ErrorMessage = "Pilih status pernikahan")]
    public string? MaritalStatus { get; set; }

    [Display(Name = "Pekerjaan")]
    public string? Occupation { get; set; }

    [Display(Name = "Pendidikan Terakhir")]
    public string? EducationLevel { get; set; }

    [Display(Name = "Golongan Darah")]
    public string? BloodType { get; set; }

    [Display(Name = "Foto (JPEG/PNG, maks 2MB)")]
    [Required(ErrorMessage = "Foto wajib diunggah")]
    public IFormFile? Photo { get; set; }

}
