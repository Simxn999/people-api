using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Interest
{
    [Key]
    public int InterestID { get; set; }

    [MaxLength(50)]
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public virtual ICollection<Person>? People { get; set; }
    public virtual ICollection<Website>? Websites { get; set; }
}