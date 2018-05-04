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
    public class CompanieService : ICompanieService
    {
        private readonly ContentManagerDbContext _context;

        public CompanieService(ContentManagerDbContext context)
        {
            _context = context;
        }

        public List<CompanieDto> FindAll(string name)
        {
            var companiesQuery = _context.Companies.ProjectTo<CompanieDto>();

            if (!string.IsNullOrWhiteSpace(name))
                companiesQuery = companiesQuery.Where(f => f.Name.ToLower().Contains(name));

            var companie = companiesQuery.ToList();

            if (companie.Count <= 0)
                return null;

            return companie;
        }

        public Companie Exist(int id)
        {
            try
            {
                var companieInDb = _context.Companies.SingleOrDefault(c => c.Id == id);
                return companieInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public Companie ExistName(string name)
        {
            try
            {
                var companieInDb = _context.Companies.SingleOrDefault(t => t.Name.ToLower() == name.ToLower());
                return companieInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public string Validate(CompanieDto companieDto)
        {

            var checkName = CheckSameName(companieDto);
            if (checkName != null)
                return checkName;

            return string.IsNullOrEmpty(companieDto.Name) ? "You send a null or empty string!" : null;
        }

        public string CheckSameName(CompanieDto companieDto)
        {
            if (!string.IsNullOrEmpty(companieDto.Name))
            {
                if (_context.Companies.Any(t => t.Name.ToLower() == companieDto.Name.ToLower()))
                    return "A Companie with that name already exist!";
            }

            return null;
        }

        public string Compare(Companie companieInDb, CompanieDto companieDto)
        {
            if (!string.IsNullOrEmpty(companieDto.Name))
            {
                if (companieInDb.Name.ToLower() == companieDto.Name.ToLower())
                    return "Same name, not changes made";

            }

            return null;
        }

        public CompanieDto Edit(Companie companieInDb, CompanieDto companieDto)
        {

            if (Compare(companieInDb, companieDto) != null)
                return Mapper.Map<Companie, CompanieDto>(companieInDb);

            if (string.IsNullOrEmpty(companieDto.Name))
                return Mapper.Map<Companie, CompanieDto>(companieInDb);

            companieInDb.Name = companieDto.Name;

            return Mapper.Map<Companie, CompanieDto>(companieInDb);
        }

        public Companie CreateNew(CompanieDto companieDto)
        {
            var newCompanie = new Companie
            {
                Name = companieDto.Name
            };
            _context.Companies.Add(newCompanie);
            return newCompanie;
        }

        public bool Erase(Companie companieInDb)
        {
            try
            {
                _context.Companies.Remove(companieInDb);
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
