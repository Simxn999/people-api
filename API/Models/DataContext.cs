using Microsoft.EntityFrameworkCore;

namespace API.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Person> People => Set<Person>();
    public DbSet<Interest> Interests => Set<Interest>();
    public DbSet<Website> Websites => Set<Website>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new DbContextOptionsBuilder().EnableSensitiveDataLogging();
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>()
                    .HasMany(person => person.Interests)
                    .WithMany(interest => interest.People)
                    .UsingEntity<Dictionary<string, object>>("PersonInterest",
                                                             b => b.HasOne<Interest>().WithMany().HasForeignKey("InterestID"),
                                                             b => b.HasOne<Person>().WithMany().HasForeignKey("PersonID"));
        modelBuilder.Entity<Person>()
                    .HasMany(person => person.Websites)
                    .WithMany(website => website.People)
                    .UsingEntity<Dictionary<string, object>>("PersonWebsite",
                                                             b => b.HasOne<Website>().WithMany().HasForeignKey("WebsiteID"),
                                                             b => b.HasOne<Person>().WithMany().HasForeignKey("PersonID"));
        modelBuilder.Entity<Interest>()
                    .HasMany(person => person.Websites)
                    .WithMany(interest => interest.Interests)
                    .UsingEntity<Dictionary<string, object>>("InterestWebsite",
                                                             b => b.HasOne<Website>().WithMany().HasForeignKey("WebsiteID"),
                                                             b => b.HasOne<Interest>().WithMany().HasForeignKey("InterestID"));

        modelBuilder.Entity<Person>()
                    .HasData(new Person
                    {
                        PersonID = 1,
                        Name = "Simon",
                        Surname = "Johansson",
                        Address = "Tvistevägen 999",
                        PhoneNumber = "123 456 78 90",
                        Email = "simon.johansson@mail.com",
                    });
        modelBuilder.Entity<Person>()
                    .HasData(new Person
                    {
                        PersonID = 2,
                        Name = "Elon",
                        Surname = "Musk",
                        Address = "Marsgatan 999",
                        PhoneNumber = "123 456 78 91",
                        Email = "elon.musk@mail.com",
                    });
        modelBuilder.Entity<Person>()
                    .HasData(new Person
                    {
                        PersonID = 3,
                        Name = "Rebecca",
                        Surname = "Gerdin",
                        Address = "Umeå 999",
                        PhoneNumber = "123 456 78 92",
                        Email = "rebecca.gerdin@mail.com",
                    });

        modelBuilder.Entity<Website>()
                    .HasData(new Website
                    {
                        WebsiteID = 1,
                        Title = "Tesla",
                        Link = "https://www.tesla.com/",
                    });

        modelBuilder.Entity<Website>()
                    .HasData(new Website
                    {
                        WebsiteID = 2,
                        Title = "SpaceX",
                        Link = "https://www.spacex.com/",
                    });

        modelBuilder.Entity<Website>()
                    .HasData(new Website
                    {
                        WebsiteID = 3,
                        Title = "FREE TESLA",
                        Link = "https://youtu.be/dQw4w9WgXcQ",
                    });

        modelBuilder.Entity<Interest>()
                    .HasData(new Interest
                    {
                        InterestID = 1,
                        Title = "Tesla",
                        Description = "Our future.",
                    });

        modelBuilder.Entity<Interest>()
                    .HasData(new Interest
                    {
                        InterestID = 2,
                        Title = "SpaceX",
                        Description = "Am big rocket.",
                    });

        modelBuilder.Entity<Interest>()
                    .HasData(new Interest
                    {
                        InterestID = 3,
                        Title = "[INSERT-TITLE-HERE]",
                        Description = "[INSERT-DESCRIPTION-HERE]",
                    });

        modelBuilder.Entity("InterestWebsite")
                    .HasData(new
                    {
                        WebsiteID = 1, InterestID = 1,
                    });
        modelBuilder.Entity("InterestWebsite")
                    .HasData(new
                    {
                        WebsiteID = 2, InterestID = 2,
                    });
        modelBuilder.Entity("InterestWebsite")
                    .HasData(new
                    {
                        WebsiteID = 3, InterestID = 3,
                    });

        modelBuilder.Entity("PersonInterest")
                    .HasData(new
                    {
                        InterestID = 1, PersonID = 1,
                    });
        modelBuilder.Entity("PersonInterest")
                    .HasData(new
                    {
                        InterestID = 1, PersonID = 2,
                    });
        modelBuilder.Entity("PersonInterest")
                    .HasData(new
                    {
                        InterestID = 2, PersonID = 1,
                    });
        modelBuilder.Entity("PersonInterest")
                    .HasData(new
                    {
                        InterestID = 2, PersonID = 2,
                    });
        modelBuilder.Entity("PersonInterest")
                    .HasData(new
                    {
                        InterestID = 3, PersonID = 3,
                    });

        modelBuilder.Entity("PersonWebsite")
                    .HasData(new
                    {
                        WebsiteID = 1, PersonID = 1,
                    });
        modelBuilder.Entity("PersonWebsite")
                    .HasData(new
                    {
                        WebsiteID = 1, PersonID = 2,
                    });
        modelBuilder.Entity("PersonWebsite")
                    .HasData(new
                    {
                        WebsiteID = 2, PersonID = 1,
                    });
        modelBuilder.Entity("PersonWebsite")
                    .HasData(new
                    {
                        WebsiteID = 2, PersonID = 2,
                    });
        modelBuilder.Entity("PersonWebsite")
                    .HasData(new
                    {
                        WebsiteID = 3, PersonID = 3,
                    });
    }
}