using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class Website
{
    [Key]
    public int WebsiteID { get; set; }

    [MaxLength(50)]
    public string Title { get; set; } = default!;

    public string Link { get; set; } = default!;

    public virtual ICollection<Person>? People { get; set; }
    public virtual ICollection<Interest>? Interests { get; set; }
}