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
    public class RegionService : IRegionService
    {
        private readonly ContentManagerDbContext _context;
        private readonly ICountryService _countryService;

        public RegionService(ContentManagerDbContext context, ICountryService countryService)
        {
            _context = context;
            _countryService = countryService;
        }

        public List<RegionDto> FindAll(string name)
        {
            var regionsQuery = _context.Regions.ProjectTo<RegionDto>();

            if (!string.IsNullOrWhiteSpace(name))
                regionsQuery = regionsQuery.Where(f => f.Name.ToLower().Contains(name));

            var regions = regionsQuery.ToList();

            if (regions.Count <= 0)
                return null;

            return regions;
        }

        public Region Exist(int id)
        {
            var regionInDb = _context.Regions.SingleOrDefault(c => c.Id == id);

            return regionInDb;
        }

        public string Validate(RegionDto regionDto)
        {

            if (!string.IsNullOrEmpty(regionDto.Name))
            {
                if (_context.Regions.Any(t => t.Name.ToLower() == regionDto.Name.ToLower()))
                    return "A Region with that name already exist!";
            }

            return string.IsNullOrEmpty(regionDto.Name) ? "You send a null or empty string!" : null;
        }

        public string Compare(Region regionInDb, RegionDto regionDto)
        {
            if (!string.IsNullOrEmpty(regionDto.Name))
            {
                if (regionInDb.Name.ToLower() == regionDto.Name.ToLower())
                    return "Same name, not changes made";

            }

            return null;
        }

        public RegionDto Edit(Region regionInDb, RegionDto regionDto)
        {

            regionInDb.Name = regionDto.Name;

            _context.SaveChanges();

            var region = _context.Regions.ProjectTo<RegionDto>().SingleOrDefault(f => f.Id == regionInDb.Id);

            return region;
        }

        public Region CreateNew(RegionDto regionDto)
        {
            var newRegion = new Region
            {
                Name = regionDto.Name
            };
            _context.Regions.Add(newRegion);
            return newRegion;
        }

        public async Task<RegionDto> SaveNew(RegionDto regionDto)
        {
            var newRegion = new Region
            {
                Name = regionDto.Name
            };
            _context.Regions.Add(newRegion);
            await _context.SaveChangesAsync();
            return Mapper.Map<Region, RegionDto>(newRegion);
        }

        public bool Erase(Region regionInDb)
        {
            _context.Regions.Remove(regionInDb);
            _context.SaveChanges();

            return true;
        }

        public async Task<RegionDto> Save(Region regionInDb)
        {
            await _context.SaveChangesAsync();
            return Mapper.Map<Region, RegionDto>(regionInDb);
        }
    }
}
