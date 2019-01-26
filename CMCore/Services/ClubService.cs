using System;
using System.Linq;
using AutoMapper;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelationModel;

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
                return "You send an invalid name!";

            return Context.Clubs.Any(c => c.Url.Equals(clubDto.Url)) ? "A club with that url already exist!" : null;
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

        public string AddRegionCountryR(Club clubInDb, ClubDto clubDto)
        {
            if (clubDto.Regions != null)
            {
                foreach (var region in clubDto.Regions)
                {
                    var countryErMsg = _regionService.AddClubR(region, clubInDb);
                    if (!string.IsNullOrWhiteSpace(countryErMsg))
                        return countryErMsg;
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

                // Validate and Add Region and/or country
                var regionCountryErrMsg = AddRegionCountryR(newClub, clubDto);
                return !string.IsNullOrWhiteSpace(regionCountryErrMsg) ? regionCountryErrMsg : null;
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
            var errMsg = AddTypeR(clubInDb, clubDto);
            if (!string.IsNullOrWhiteSpace(errMsg))
                return errMsg;

            // Validate and Add Region and/or country
            var rCountryErrMsg = AddRegionCountryR(clubInDb, clubDto);
            return !string.IsNullOrWhiteSpace(rCountryErrMsg) ? rCountryErrMsg : null;
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
