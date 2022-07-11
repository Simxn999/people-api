using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class PersonWebsitesRepository : ISubRepository<Website>
{
    readonly DataContext _context;

    public PersonWebsitesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Website>> Get(int personID)
    {
        var person = _context.People.FirstOrDefault(p => p.PersonID == personID);
        if (person is null) return Enumerable.Empty<Website>();

        var websites = await _context.Websites.Include(i => i.People).Include(i => i.Interests).Where(i => i.People!.Contains(person)).ToListAsync();

        return websites.Select(WebsitesRepository.FilterWebsite);
    }

    public async Task<Website?> Add(int personID, int entityID)
    {
        var person = await _context.People.Include(p => p.Interests).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == personID);

        var website = await _context.Websites.Include(w => w.People).Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == entityID);

        if (person is null || website is null || person.Websites!.Any(i => i.WebsiteID == entityID)) return null;

        person.Websites!.Add(website);
        await _context.SaveChangesAsync();

        return WebsitesRepository.FilterWebsite(website);
    }

    public async Task<Website?> Delete(int personID, int entityID)
    {
        var person = await _context.People.Include(p => p.Websites).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == personID);

        var website = await _context.Websites.Include(w => w.People).Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == entityID);

        if (person is null || website is null || person.Websites!.All(w => w.WebsiteID != entityID))
            return null;

        person.Websites!.Remove(website);
        await _context.SaveChangesAsync();

        return WebsitesRepository.FilterWebsite(website);
    }
}