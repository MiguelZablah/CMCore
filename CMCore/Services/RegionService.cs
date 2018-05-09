using System.Linq;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelacionClass;

namespace CMCore.Services
{
    public class RegionService : GenericService<Region, RegionDto>, IRegionService
    {
        private readonly ICountryService _countryService;

        public RegionService(ContentManagerDbContext context, ICountryService countryService) : base(context)
        {
            _countryService = countryService;
        }

        public Region CreateNew(RegionDto regionDto)
        {
            var newRegion = new Region
            {
                Name = regionDto.Name
            };

            return AddEf(newRegion) ? newRegion : default(Region);
        }

        public string AddCountrieR(Region regionInDb, RegionDto regionDto)
        {
            if (regionDto.Countries != null)
            {
                foreach (var countrie in regionDto.Countries)
                {
                    var countrieErMsg = _countryService.AddRegionR(countrie, regionInDb);
                    if (countrieErMsg != null)
                        return countrieErMsg;
                }
                return null;
            }

            return null;
        }

        public string AddClubCountrieR(RegionDto regionDto, Club clubInDb)
        {
            var regionInDb = ExistName(regionDto.Name).FirstOrDefault();
            if (regionInDb == null)
            {
                var createdRegion = new Region
                {
                    Name = regionDto.Name
                };
                AddEf(createdRegion);

                var newRegion = createdRegion;
                var newClubRegionRelation = new ClubRegion
                {
                    ClubId = clubInDb.Id,
                    RegionId = newRegion.Id
                };
                Context.ClubRegions.Add(newClubRegionRelation);

                // Add countrie relacion
                var countryValMsg = AddCountrieR(newRegion, regionDto);
                if (countryValMsg != null)
                    return countryValMsg;

                return null;
            }

            if (string.IsNullOrEmpty(regionDto.Name))
                return "You send a null or empty region!";

            var regionHasClub = clubInDb.ClubRegions.Any(cr => cr.RegionId == regionInDb.Id);
            if (!regionHasClub)
            {
                var newClubRegionRelation = new ClubRegion
                {
                    ClubId = clubInDb.Id,
                    RegionId = regionInDb.Id
                };
                Context.ClubRegions.Add(newClubRegionRelation);
            }

            // Add countrie relacion
            var countryValMsgg = AddCountrieR(regionInDb, regionDto);
            if (countryValMsgg != null)
                return countryValMsgg;

            return null;

        }

    }
}
