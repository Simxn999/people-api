using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class InterestsRepository : IRepository<Interest>
{
    readonly DataContext _context;

    public InterestsRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Interest>> GetAll()
    {
        var interests = await _context.Interests.Include(i => i.People).Include(i => i.Websites).ToListAsync();
        return interests.Select(FilterInterest);
    }

    public async Task<Interest?> Get(int interestID)
    {
        var interest = await _context.Interests.Include(i => i.People).Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == interestID);

        return interest is null ? null : FilterInterest(interest);
    }

    public async Task<Interest?> Add(Interest entity)
    {
        var interest = new Interest
        {
            Title = entity.Title,
            Description = entity.Description,
            People = new List<Person>(),
            Websites = new List<Website>(),
        };

        if (entity.People is not null)
            foreach (var person in entity.People)
            {
                var personEntity = await _context.People.Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == person.PersonID) ??
                                   _context.People.Add(person).Entity;

                interest.People.Add(personEntity);
            }

        if (entity.Websites is not null)
            foreach (var website in entity.Websites)
            {
                var websiteEntity = await _context.Websites.Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == website.WebsiteID) ??
                                    _context.Websites.Add(website).Entity;

                interest.Websites.Add(websiteEntity);
            }

        foreach (var person in interest.People)
        foreach (var interestWebsite in interest.Websites)
            (person.Websites ??= new List<Website>()).Add(interestWebsite);

        var result = await _context.Interests.AddAsync(interest);

        await _context.SaveChangesAsync();

        return await Get(result.Entity.InterestID);
    }

    public async Task<Interest?> Update(Interest entity)
    {
        var interest = await _context.Interests.Include(i => i.People)
                                     .Include(i => i.Websites)
                                     .FirstOrDefaultAsync(i => i.InterestID == entity.InterestID);

        if (interest is null)
            return null;

        interest.Title = entity.Title;
        interest.Description = entity.Description;
        interest.People = entity.People;
        interest.Websites = entity.Websites;

        await _context.SaveChangesAsync();

        return FilterInterest(interest);
    }

    public async Task<Interest?> Delete(int interestID)
    {
        var interest = await _context.Interests.FirstOrDefaultAsync(i => i.InterestID == interestID);

        if (interest is null)
            return null;

        _context.Interests.Remove(interest);
        await _context.SaveChangesAsync();

        return FilterInterest(interest);
    }

    public static Interest FilterInterest(Interest i)
    {
        return new Interest
        {
            InterestID = i.InterestID,
            Title = i.Title,
            Description = i.Description,
            People = i.People?.Select(p => new Person
                      {
                          PersonID = p.PersonID,
                          Name = p.Name,
                          Surname = p.Surname,
                          Address = p.Address,
                          PhoneNumber = p.PhoneNumber,
                          Email = p.Email,
                      })
                      .ToList(),
            Websites = i.Websites?.Select(w => new Website
                        {
                            WebsiteID = w.WebsiteID,
                            Title = w.Title,
                            Link = w.Link,
                        })
                        .ToList(),
        };
    }
}