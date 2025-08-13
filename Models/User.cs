using System.ComponentModel.DataAnnotations;

namespace UserProfileManager.Models;

public class User
{
    public int Id { get; set; }

    // Identitas
    [Required, StringLength(120)]
    public string FullName { get; set; } = default!;

    [Required, StringLength(32)]
    public string NIK { get; set; } = default!;

    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Phone]
    public string? Phone { get; set; }

    [Required]
    public string? Gender { get; set; }

    [StringLength(80)]
    public string? BirthPlace { get; set; }

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    // Alamat
    [StringLength(200)]
    public string? Address { get; set; }
    [StringLength(80)]
    public string? City { get; set; }
    [StringLength(80)]
    public string? Province { get; set; }
    [StringLength(10)]
    public string? PostalCode { get; set; }
    [StringLength(60)]
    public string? Nationality { get; set; }

    // Latar belakang
    [StringLength(20)]
    public string? MaritalStatus { get; set; }
    [StringLength(80)]
    public string? Occupation { get; set; }
    [StringLength(60)]
    public string? EducationLevel { get; set; }
    [StringLength(3)]
    public string? BloodType { get; set; }

    // Foto
    public string? PhotoFileName { get; set; }

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
