using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCore.Services
{
    public class ClubService : IClubService
    {
        private readonly ContentManagerDbContext _context;

        public ClubService(ContentManagerDbContext context)
        {
            _context = context;
        }

        public List<ClubDto> FindAll(string name)
        {
            var clubQuery = _context.Clubs.ProjectTo<ClubDto>();

            if (!string.IsNullOrWhiteSpace(name))
                clubQuery = clubQuery.Where(f => f.Name.ToLower().Contains(name));

            var clubs = clubQuery.ToList();

            if (clubs.Count <= 0)
                return null;

            return clubs;
        }

        public Club Exist(int id)
        {
            try
            {
                var clubInDb = _context.Clubs
                    .Include(c => c.ClubTypes)
                    .ThenInclude(ct => ct.Type)
                    .Include(c => c.ClubRegions)
                    .ThenInclude(cr => cr.Region)
                    .ThenInclude(r => r.Countries)
                    .SingleOrDefault(c => c.Id == id);
                return clubInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public Club ExistName(string name)
        {
            try
            {
                var clubInDb = _context.Clubs
                    .Include(c => c.ClubTypes)
                    .ThenInclude(ct => ct.Type)
                    .Include(c => c.ClubRegions)
                    .ThenInclude(cr => cr.Region)
                    .ThenInclude(r => r.Countries)
                    .SingleOrDefault(c => c.Name.ToLower() == name.ToLower());
                return clubInDb;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public string Validate(ClubDto clubDto)
        {
            throw new NotImplementedException();
        }

        public string CheckSameName(ClubDto clubDto)
        {
            throw new NotImplementedException();
        }

        public string Compare(Club clubInDb, ClubDto clubDto)
        {
            throw new NotImplementedException();
        }

        public ClubDto Edit(Club clubInDb, ClubDto clubDto)
        {
            throw new NotImplementedException();
        }

        public Club CreateNew(ClubDto clubDto)
        {
            throw new NotImplementedException();
        }

        public bool Erase(Club clubInDb)
        {
            throw new NotImplementedException();
        }
    }
}
