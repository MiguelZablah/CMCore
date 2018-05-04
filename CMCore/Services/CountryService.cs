using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                var countrieInDb = _context.Countries.SingleOrDefault(c => c.Id == id);
                return countrieInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public Countrie ExistName(string name)
        {
            try
            {
                var countrieInDb = _context.Countries.SingleOrDefault(c => c.Name.ToLower() == name.ToLower());
                return countrieInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public string Validate(CountrieDto countriDto)
        {

            var checkName = CheckSameName(countriDto);
            if (checkName != null)
                return checkName;

            return string.IsNullOrEmpty(countriDto.Name) ? "You send a null or empty string!" : null;
        }

        public string CheckSameName(CountrieDto countriDto)
        {
            if (!string.IsNullOrEmpty(countriDto.Name))
            {
                if (_context.Countries.Any(t => t.Name.ToLower() == countriDto.Name.ToLower()))
                    return "A Countrie with that name already exist!";
            }

            return null;
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

            if (Compare(countrieInDb, countrieDto) != null)
                return Mapper.Map<Countrie, CountrieDto>(countrieInDb);

            if (string.IsNullOrEmpty(countrieDto.Name))
                return Mapper.Map<Countrie, CountrieDto>(countrieInDb);

            countrieInDb.Name = countrieDto.Name;

            return Mapper.Map<Countrie, CountrieDto>(countrieInDb);
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

            var regionHasCountry = regionInDb.Countries.Any(cr => cr.RegionId == countrieInDb.RegionId);
            if (!regionHasCountry)
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

        public bool Erase(Countrie countrieInDb)
        {
            try
            {
                _context.Countries.Remove(countrieInDb);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
