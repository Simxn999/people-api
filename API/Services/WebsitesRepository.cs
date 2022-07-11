using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class WebsitesRepository : IRepository<Website>
{
    readonly DataContext _context;

    public WebsitesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Website>> GetAll()
    {
        var websites = await _context.Websites.Include(w => w.People).Include(w => w.Interests).ToListAsync();

        return websites.Select(FilterWebsite);
    }

    public async Task<Website?> Get(int websiteID)
    {
        var website = await _context.Websites.Include(w => w.People).Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == websiteID);

        return website is null ? null : FilterWebsite(website);
    }

    public async Task<Website?> Add(Website entity)
    {
        var website = new Website
        {
            Title = entity.Title,
            Link = entity.Link,
            People = new List<Person>(),
            Interests = new List<Interest>(),
        };

        if (entity.People is not null)
            foreach (var person in entity.People)
            {
                var personEntity = await _context.People.Include(p => p.Interests).FirstOrDefaultAsync(p => p.PersonID == person.PersonID) ??
                                   _context.People.Add(person).Entity;

                website.People.Add(personEntity);
            }

        if (entity.Interests is not null)
            foreach (var interest in entity.Interests)
            {
                var interestEntity = await _context.Interests.Include(i => i.People).FirstOrDefaultAsync(i => i.InterestID == interest.InterestID) ??
                                     _context.Interests.Add(interest).Entity;

                website.Interests.Add(interestEntity);

                if (interestEntity.People is null) continue;

                foreach (var person in interestEntity.People) website.People.Add(person);
            }

        var result = await _context.Websites.AddAsync(website);

        await _context.SaveChangesAsync();

        return await Get(result.Entity.WebsiteID);
    }

    public async Task<Website?> Update(Website entity)
    {
        var website = await _context.Websites.FirstOrDefaultAsync(w => w.WebsiteID == entity.WebsiteID);

        if (website is null)
            return null;

        website.Title = entity.Title;
        website.Link = entity.Link;
        website.People = entity.People;
        website.Interests = entity.Interests;

        await _context.SaveChangesAsync();

        return FilterWebsite(website);
    }

    public async Task<Website?> Delete(int websiteID)
    {
        var website = await _context.Websites.FirstOrDefaultAsync(w => w.WebsiteID == websiteID);

        if (website is null)
            return null;

        _context.Websites.Remove(website);
        await _context.SaveChangesAsync();

        return FilterWebsite(website);
    }

    public static Website FilterWebsite(Website w)
    {
        return new Website
        {
            WebsiteID = w.WebsiteID,
            Title = w.Title,
            Link = w.Link,
            People = w.People?.Select(p => new Person
                      {
                          PersonID = p.PersonID,
                          Name = p.Name,
                          Surname = p.Surname,
                          Address = p.Address,
                          PhoneNumber = p.PhoneNumber,
                          Email = p.Email,
                      })
                      .ToList(),
            Interests = w.Interests?.Select(i => new Interest
                         {
                             InterestID = i.InterestID,
                             Title = i.Title,
                             Description = i.Description,
                         })
                         .ToList(),
        };
    }
}