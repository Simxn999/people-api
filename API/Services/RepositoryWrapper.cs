using API.Models;

namespace API.Services;

public class RepositoryWrapper : IRepositoryWrapper
{
    readonly DataContext _context;
    ISubRepository<Person>? _interestPeople;
    IRepository<Interest>? _interests;
    ISubRepository<Website>? _interestWebsites;
    IRepository<Person>? _people;
    ISubRepository<Interest>? _personInterests;
    ISubRepository<Website>? _personWebsites;
    ISubRepository<Interest>? _websiteInterests;
    ISubRepository<Person>? _websitePeople;
    IRepository<Website>? _websites;

    public RepositoryWrapper(DataContext context)
    {
        _context = context;
    }

    public ISubRepository<Person> InterestPeople => _interestPeople ??= new InterestPeopleRepository(_context);
    public IRepository<Interest> Interests => _interests ??= new InterestsRepository(_context);
    public ISubRepository<Website> InterestWebsites => _interestWebsites ??= new InterestWebsitesRepository(_context);
    public IRepository<Person> People => _people ??= new PeopleRepository(_context);
    public ISubRepository<Interest> PersonInterests => _personInterests ??= new PersonInterestsRepository(_context);
    public ISubRepository<Website> PersonWebsites => _personWebsites ??= new PersonWebsitesRepository(_context);
    public ISubRepository<Interest> WebsiteInterests => _websiteInterests ??= new WebsiteInterestsRepository(_context);
    public ISubRepository<Person> WebsitePeople => _websitePeople ??= new WebsitePeopleRepository(_context);
    public IRepository<Website> Websites => _websites ??= new WebsitesRepository(_context);
}