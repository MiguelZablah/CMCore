using System;
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
                    if (!string.IsNullOrWhiteSpace(countrieErMsg))
                        return countrieErMsg;
                }
                return null;
            }

            return null;
        }

        public string AddClubR(RegionDto regionDto, Club clubInDb)
        {
            var regionInDb = ExistName(regionDto.Name, true).FirstOrDefault();
            if (regionInDb == null)
            {
                var createdRegion = CreateNew(regionDto);
                if (createdRegion == null)
                    return "Region couden't be created!";

                var newRegion = createdRegion;
                var newClubRegionRelation = new ClubRegion
                {
                    ClubId = clubInDb.Id,
                    RegionId = newRegion.Id
                };
                Context.ClubRegions.AddAsync(newClubRegionRelation);

                // Add countrie relacion
                var countryValMsg = AddCountrieR(newRegion, regionDto);
                if (!string.IsNullOrWhiteSpace(countryValMsg))
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
                Context.ClubRegions.AddAsync(newClubRegionRelation);
            }

            // Add countrie relacion
            var countryValMsgg = AddCountrieR(regionInDb, regionDto);
            if (!string.IsNullOrWhiteSpace(countryValMsgg))
                return countryValMsgg;

            return null;
        }

        public bool ClearRelations(Region regionInDb)
        {
            try
            {
                regionInDb.Countries.Clear();

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
