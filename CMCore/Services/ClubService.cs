using System;
using System.Linq;
using AutoMapper;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelacionClass;
using Microsoft.EntityFrameworkCore;

namespace CMCore.Services
{
    public class ClubService : GenericService<Club, ClubDto>, IClubService
    {
        private readonly IRegionService _regionService;
        private readonly ITypeService _typeService;

        public ClubService(ContentManagerDbContext context, IRegionService regionService, ITypeService typeService) : base(context)
        {
            _regionService = regionService;
            _typeService = typeService;
        }

        public override ClubDto Edit(Club clubInDb, ClubDto clubDto)
        {
            if (CompareString(clubInDb.Name, clubDto.Name))
                clubInDb.Name = clubDto.Name;

            if (CompareString(clubInDb.Url, clubDto.Url))
                clubInDb.Url = clubDto.Url;

            if (string.IsNullOrWhiteSpace(clubInDb.Name))
                clubInDb.Name = clubInDb.Name;

            if (string.IsNullOrWhiteSpace(clubInDb.Url))
                clubInDb.Url = clubDto.Url;

            return Mapper.Map<Club, ClubDto>(clubInDb);
        }

        public override string Validate(ClubDto clubDto)
        {
            var checkName = CheckSameName(clubDto.Name);
            if (!string.IsNullOrWhiteSpace(checkName))
                return checkName;

            if (string.IsNullOrWhiteSpace(clubDto.Name))
                return "You send am invalid name!";

            if (Context.Clubs.Any(c => c.Url.Equals(clubDto.Url)))
                return "A club with that url already exist!";

            return null;
        }

        public Club CreateNew(ClubDto clubDto)
        {
            var newClub = new Club
            {
                Name = clubDto.Name,
                Url = clubDto.Url
            };

            return AddEf(newClub) ? newClub : default(Club);
        }

        public string AddRegionCountriR(Club clubInDb, ClubDto clubDto)
        {
            if (clubDto.Regions != null)
            {
                foreach (var region in clubDto.Regions)
                {
                    var countrieErMsg = _regionService.AddClubR(region, clubInDb);
                    if (!string.IsNullOrWhiteSpace(countrieErMsg))
                        return countrieErMsg;
                }
                return null;
            }

            return null;
        }

        public string AddTypeR(Club clubInDb, ClubDto clubDto)
        {
            if (clubDto.Types != null)
            {
                foreach (var type in clubDto.Types)
                {
                    var typeErMsg = _typeService.AddClubR(type, clubInDb);
                    if (!string.IsNullOrWhiteSpace(typeErMsg))
                        return typeErMsg;
                }
                return null;
            }

            return null;
        }

        public string AddFileR(ClubDto clubDto, File fileInDb)
        {
            var clubInDb = ExistName(clubDto.Name, true).FirstOrDefault();
            if (clubInDb == null)
            {
                var createdClub = CreateNew(clubDto);
                if (createdClub == null)
                    return "Could not create Club";

                var newClub = createdClub;
                var newFileClub = new FileClub
                {
                    FileId = fileInDb.Id,
                    ClubId = newClub.Id
                };
                Context.FileClubs.AddAsync(newFileClub);

                // Validate and Add Types
                var typesErrMsg = AddTypeR(newClub, clubDto);
                if (!string.IsNullOrWhiteSpace(typesErrMsg))
                    return typesErrMsg;

                // Validate and Add Region and/or countrie
                var regionCountriErrMsg = AddRegionCountriR(newClub, clubDto);
                if (!string.IsNullOrWhiteSpace(regionCountriErrMsg))
                    return regionCountriErrMsg;


                return null;
            }

            if (string.IsNullOrEmpty(clubDto.Name))
                return "You send a null or empty Club!";

            if (!fileInDb.FileClubs.Any(cr => cr.ClubId == clubInDb.Id))
            {
                var newFileClub = new FileClub
                {
                    FileId = fileInDb.Id,
                    ClubId = clubInDb.Id
                };
                Context.FileClubs.AddAsync(newFileClub);
            }

            // Validate and Add Types
            var typesErrMsgg = AddTypeR(clubInDb, clubDto);
            if (!string.IsNullOrWhiteSpace(typesErrMsgg))
                return typesErrMsgg;

            // Validate and Add Region and/or countrie
            var regionCountriErrMsgg = AddRegionCountriR(clubInDb, clubDto);
            if (!string.IsNullOrWhiteSpace(regionCountriErrMsgg))
                return regionCountriErrMsgg;

            return null;
        }

        public bool ClearRelations(Club clubInDb)
        {
            try
            {
                clubInDb.ClubTypes.Clear();
                clubInDb.ClubRegions.Clear();
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
