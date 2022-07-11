using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class WebsitePeopleRepository : ISubRepository<Person>
{
    readonly DataContext _context;

    public WebsitePeopleRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Person>> Get(int websiteID)
    {
        var website = _context.Websites.FirstOrDefault(w => w.WebsiteID == websiteID);
        if (website is null) return Enumerable.Empty<Person>();

        var people = await _context.People.Include(p => p.Interests).Include(p => p.Websites).Where(p => p.Websites!.Contains(website)).ToListAsync();

        return people.Select(PeopleRepository.FilterPerson);
    }

    public async Task<Person?> Add(int websiteID, int entityID)
    {
        var website = await _context.Websites.Include(w => w.People).Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == websiteID);

        var person = await _context.People.Include(p => p.Interests).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == entityID);

        if (website is null || person is null || website.People!.Any(p => p.PersonID == entityID)) return null;

        website.People!.Add(person);
        await _context.SaveChangesAsync();

        return PeopleRepository.FilterPerson(person);
    }

    public async Task<Person?> Delete(int websiteID, int entityID)
    {
        var website = await _context.Websites.Include(w => w.People).Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == websiteID);

        var person = await _context.People.Include(p => p.Interests).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == entityID);

        if (website is null || person is null || website.People!.All(p => p.PersonID != entityID))
            return null;

        website.People!.Remove(person);
        await _context.SaveChangesAsync();

        return PeopleRepository.FilterPerson(person);
    }
}