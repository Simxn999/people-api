using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Person
{
    [Key]
    public int PersonID { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = default!;

    [MaxLength(50)]
    public string Surname { get; set; } = default!;

    [MaxLength(100)]
    public string? Address { get; set; }

    [MaxLength(32)]
    public string? PhoneNumber { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    public virtual ICollection<Interest>? Interests { get; set; }
    public virtual ICollection<Website>? Websites { get; set; }
}