using System;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;

namespace CMCore.Services
{
    public class ClubService : GenericService<Club, ClubDto>, IClubService
    {
        public ClubService(ContentManagerDbContext context) : base(context)
        {
        }

        //public Club Exist(int id)
        //{
        //    try
        //    {
        //        var clubInDb = _context.Clubs
        //            .Include(c => c.ClubTypes)
        //                .ThenInclude(ct => ct.Type)
        //            .Include(c => c.ClubRegions)
        //                .ThenInclude(cr => cr.Region)
        //                    .ThenInclude(r => r.Countries)
        //            .SingleOrDefault(c => c.Id == id);
        //        return clubInDb;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return null;
        //    }
        //}

        //public Club ExistName(string name)
        //{
        //    try
        //    {
        //        var clubInDb = _context.Clubs
        //            .Include(c => c.ClubTypes)
        //            .ThenInclude(ct => ct.Type)
        //            .Include(c => c.ClubRegions)
        //            .ThenInclude(cr => cr.Region)
        //            .ThenInclude(r => r.Countries)
        //            .SingleOrDefault(c => c.Name.ToLower() == name.ToLower());
        //        return clubInDb;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        return null;
        //    }
        //}

        public ClubDto Edit(Club clubInDb, ClubDto clubDto)
        {
            throw new NotImplementedException();
        }

        public Club CreateNew(ClubDto clubDto)
        {
            throw new NotImplementedException();
        }
    }
}
