using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;

namespace CMCore.Services
{
    public class CountryService : ICountryService
    {
        private readonly ContentManagerDbContext _context;

        public CountryService(ContentManagerDbContext context)
        {
            _context = context;
        }

        public List<CountrieDto> FindAll(string name)
        {
            var countriesQuery = _context.Countries.ProjectTo<CountrieDto>();

            if (!string.IsNullOrWhiteSpace(name))
                countriesQuery = countriesQuery.Where(f => f.Name.ToLower().Contains(name));

            var countries = countriesQuery.ToList();

            if (countries.Count <= 0)
                return null;

            return countries;
        }

        public Countrie Exist(int id)
        {
            var countrieInDb = _context.Countries.SingleOrDefault(c => c.Id == id);

            return countrieInDb;
        }

        public Countrie ExistName(string name)
        {
            var countrieInDb = _context.Countries.SingleOrDefault(c => c.Name.ToLower() == name.ToLower());

            return countrieInDb;
        }

        public string Validate(CountrieDto countrieDto)
        {

            if (!string.IsNullOrEmpty(countrieDto.Name))
            {
                if (_context.Countries.Any(t => t.Name.ToLower() == countrieDto.Name.ToLower()))
                    return "A Country with that name already exist!";
            }

            return string.IsNullOrEmpty(countrieDto.Name) ? "You send a null or empty string!" : null;
        }

        public string Compare(Countrie countrieInDb, CountrieDto countrieDto)
        {
            if (!string.IsNullOrEmpty(countrieDto.Name))
            {
                if (countrieInDb.Name.ToLower() == countrieDto.Name.ToLower())
                    return "Same name, not changes made";

            }

            return null;
        }

        public CountrieDto Edit(Countrie countrieInDb, CountrieDto countrieDto)
        {

            Mapper.Map(countrieDto, countrieInDb);

            _context.SaveChanges();

            var countrie = _context.Countries.ProjectTo<CountrieDto>().SingleOrDefault(f => f.Id == countrieInDb.Id);

            return countrie;
        }

        public string EditSaveRegionR(CountrieDto countrieDto, Region regionInDb)
        {
            var countrieInDb = ExistName(countrieDto.Name);
            if (countrieInDb == null)
            {
                var createCountry = new Countrie
                {
                    Name = countrieDto.Name,
                    RegionId = regionInDb.Id
                };
                _context.Countries.Add(createCountry);
                return null;
            }

            if (string.IsNullOrEmpty(countrieDto.Name))
                return "You send a null or empty countrie!";

            var regionHasCountry = regionInDb.Countries.SingleOrDefault(cr => cr.RegionId == countrieInDb.RegionId);
            if (regionHasCountry == null)
            {
                countrieInDb.RegionId = regionInDb.Id;
            }

            return null;

        }

        public Countrie CreateNew(CountrieDto countrieDto)
        {
            var newCountrie = new Countrie
            {
                Name = countrieDto.Name
            };
            _context.Countries.Add(newCountrie);
            return newCountrie;
        }

        public async Task<CountrieDto> SaveNew(CountrieDto countrieDto)
        {
            var newCountrie = new Countrie
            {
                Name = countrieDto.Name
            };
            _context.Countries.Add(newCountrie);
            await _context.SaveChangesAsync();
            return Mapper.Map<Countrie, CountrieDto>(newCountrie);
        }

        public bool Erase(Countrie countrieInDb)
        {
            _context.Countries.Remove(countrieInDb);
            _context.SaveChanges();

            return true;
        }
    }
}
