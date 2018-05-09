using System.Linq;
using AutoMapper;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;

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

            return Mapper.Map<Club, ClubDto>(clubInDb);
        }

        public override string Validate(ClubDto clubDto)
        {
            var checkName = CheckSameName(clubDto);
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
                    var countrieErMsg = _regionService.AddClubCountrieR(region, clubInDb);
                    if (countrieErMsg != null)
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
                    var countrieErMsg = _typeService.AddClubR(type, clubInDb);
                    if (countrieErMsg != null)
                        return countrieErMsg;
                }
                return null;
            }

            return null;
        }

    }
}
