using System;
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
            var CountriesQuery = _context.Countries.ProjectTo<CountrieDto>();

            if (!string.IsNullOrWhiteSpace(name))
                CountriesQuery = CountriesQuery.Where(f => f.Name.ToLower().Contains(name));

            var Countries = CountriesQuery.ToList();

            if (Countries.Count <= 0)
                return null;

            return Countries;
        }

        public Countrie Exist(int id)
        {
            var countrieInDb = _context.Countries.SingleOrDefault(c => c.Id == id);

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

            var Countrie = _context.Countries.ProjectTo<CountrieDto>().SingleOrDefault(f => f.Id == countrieInDb.Id);

            return Countrie;
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
