using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class PeopleRepository : IRepository<Person>
{
    readonly DataContext _context;

    public PeopleRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Person>> GetAll()
    {
        var people = await _context.People.Include(p => p.Interests).Include(p => p.Websites).ToListAsync();

        return people.Select(FilterPerson);
    }

    public async Task<Person?> Get(int personID)
    {
        var person = await _context.People.Include(p => p.Interests).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == personID);

        return person is null ? null : FilterPerson(person);
    }

    public async Task<Person?> Add(Person entity)
    {
        var person = new Person
        {
            Name = entity.Name,
            Surname = entity.Surname,
            Address = entity.Address,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            Interests = new List<Interest>(),
            Websites = new List<Website>(),
        };

        if (entity.Interests is not null)
            foreach (var interest in entity.Interests)
            {
                var interestEntity =
                    await _context.Interests.Include(i => i.Websites).FirstOrDefaultAsync(i => i.InterestID == interest.InterestID) ??
                    _context.Interests.Add(interest).Entity;

                person.Interests.Add(interestEntity);

                if (interestEntity.Websites is null) continue;

                foreach (var interestWebsite in interestEntity.Websites) person.Websites.Add(interestWebsite);
            }

        if (entity.Websites is not null)
            foreach (var website in entity.Websites)
            {
                var websiteEntity = await _context.Websites.Include(w => w.Interests).FirstOrDefaultAsync(w => w.WebsiteID == website.WebsiteID) ??
                                    _context.Websites.Add(website).Entity;

                person.Websites.Add(websiteEntity);
            }

        var result = await _context.People.AddAsync(person);

        await _context.SaveChangesAsync();

        return await Get(result.Entity.PersonID);
    }

    public async Task<Person?> Update(Person entity)
    {
        var person = await _context.People.Include(p => p.Interests).Include(p => p.Websites).FirstOrDefaultAsync(p => p.PersonID == entity.PersonID);

        if (person is null)
            return null;

        person.Name = entity.Name;
        person.Surname = entity.Surname;
        person.Address = entity.Address;
        person.PhoneNumber = entity.PhoneNumber;
        person.Email = entity.Email;
        person.Interests = entity.Interests;
        person.Websites = entity.Websites;

        await _context.SaveChangesAsync();

        return FilterPerson(person);
    }

    public async Task<Person?> Delete(int personID)
    {
        var person = await _context.People.FirstOrDefaultAsync(p => p.PersonID == personID);

        if (person is null)
            return null;

        _context.People.Remove(person);
        await _context.SaveChangesAsync();

        return FilterPerson(person);
    }

    public static Person FilterPerson(Person p)
    {
        return new Person
        {
            PersonID = p.PersonID,
            Name = p.Name,
            Surname = p.Surname,
            Address = p.Address,
            PhoneNumber = p.PhoneNumber,
            Email = p.Email,
            Interests = p.Interests?.Select(i => new Interest
                         {
                             InterestID = i.InterestID,
                             Title = i.Title,
                             Description = i.Description,
                         })
                         .ToList(),
            Websites = p.Websites?.Select(w => new Website
                        {
                            WebsiteID = w.WebsiteID,
                            Title = w.Title,
                            Link = w.Link,
                        })
                        .ToList(),
        };
    }
}