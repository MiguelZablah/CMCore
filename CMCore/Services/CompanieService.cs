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
            var companieInDb = _context.Companies.SingleOrDefault(c => c.Id == id);

            return companieInDb;
        }

        public string Validate(CompanieDto companieDto)
        {

            if (!string.IsNullOrEmpty(companieDto.Name))
            {
                if (_context.Companies.Any(t => t.Name.ToLower() == companieDto.Name.ToLower()))
                    return "A Companie with that name already exist!";
            }

            return string.IsNullOrEmpty(companieDto.Name) ? "You send a null or empty string!" : null;
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

            Mapper.Map(companieDto, companieInDb);

            _context.SaveChanges();

            var companie = _context.Companies.ProjectTo<CompanieDto>().SingleOrDefault(f => f.Id == companieInDb.Id);

            return companie;
        }

        public async Task<CompanieDto> SaveNew(CompanieDto companieDto)
        {
            var newCompanie = new Companie
            {
                Name = companieDto.Name
            };
            _context.Companies.Add(newCompanie);
            await _context.SaveChangesAsync();
            return Mapper.Map<Companie, CompanieDto>(newCompanie);
        }

        public bool Erase(Companie companieInDb)
        {
            _context.Companies.Remove(companieInDb);
            _context.SaveChanges();

            return true;
        }

    }
}
