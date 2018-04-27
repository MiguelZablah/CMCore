using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("region/")]
    [EnableCors("AllowSpecificOrigin")]
    public class RegionController : Controller
    {
        private readonly ContentManagerDbContext _context;

        public RegionController(ContentManagerDbContext context)
        {
            _context = context;
        }

        // GET region/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var regionsQuery = _context.Regions.ProjectTo<RegionDto>();

            if(!String.IsNullOrWhiteSpace(name))
                regionsQuery = regionsQuery.Where(f => f.Name.ToLower().Contains(name));

            var regions = regionsQuery.ToList();

            if (regions.Count <= 0)
                return BadRequest("No Regions");

            return Ok(regions);
        }

        // GET region/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var region = _context.Regions.ProjectTo<RegionDto>().SingleOrDefault(r => r.Id == id);

            if (region == null)
                return BadRequest("Region not found");

            return Ok(region);
        }

        // PATCH region/id
        [HttpPatch("{id}")]
        public IActionResult Edit(int id, [FromBody] RegionDto regionDto)
        {
            if (regionDto == null)
                return BadRequest("Did you realy send a region?");

            var regionInDb = _context.Regions.SingleOrDefault(r => r.Id == id);

            if (regionInDb == null)
                return NotFound();

            if (!String.IsNullOrEmpty(regionDto.Name))
            {
                if (regionInDb.Name.ToLower() == regionDto.Name.ToLower())
                    return BadRequest("Same name, not changes made");

                if (_context.Regions.Any(t => t.Name.ToLower() == regionDto.Name.ToLower()))
                    return BadRequest("A Tag with that name already exist!");
            }

            // Keep name if not send
            if (String.IsNullOrEmpty(regionDto.Name))
                regionDto.Name = regionInDb.Name;

            // Countrie check
            if (regionDto.Countries != null)
            {
                foreach (var country in regionDto.Countries)
                {
                    if (String.IsNullOrEmpty(country.Name))
                        return BadRequest("Country with no name! That is bad!");

                    var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name.ToLower() == country.Name.ToLower());
                    if (existingCountry == null)
                    {
                        var createCountry = new Countrie
                        {
                            Name = country.Name,
                            RegionId = regionInDb.Id
                        };
                        _context.Countries.Add(createCountry);
                    }
                    else
                    {
                        var regionHasCountry = regionInDb.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
                        if (regionHasCountry)
                        {
                            existingCountry.RegionId = regionInDb.Id;
                        }
                    }
                }
            }

            Mapper.Map(regionDto, regionInDb);

            _context.SaveChanges();

            // Return new file
            var region = _context.Regions.ProjectTo<RegionDto>().SingleOrDefault(f => f.Id == regionInDb.Id);

            return Ok(region);
        }

        // Delete region/delete/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var regionInDb = _context.Regions.SingleOrDefault(c => c.Id == id);

            if (regionInDb == null)
                return NotFound();

            _context.Regions.Remove(regionInDb);
            _context.SaveChanges();

            return Ok("Region Deleted: " + id);
        }

        // POST region/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] RegionDto regionDto)
        {
            if (regionDto == null)
                return BadRequest("Did you send one Region or somthing else?!");

            if (String.IsNullOrEmpty(regionDto.Name))
                return BadRequest("Not Region name send!");

            if (_context.Regions.Any(t => t.Name.ToLower() == regionDto.Name.ToLower()))
                return BadRequest("Region name already exist! No duplicates plz!");

            var createdRegion = new Region
            {
                Name = regionDto.Name
            };
            _context.Regions.Add(createdRegion);
            var newRegion = createdRegion;

            // Countries check
            if (regionDto.Countries != null)
            {
                foreach (var country in regionDto.Countries)
                {
                    if (String.IsNullOrEmpty(country.Name))
                        return BadRequest("Country with no name! That is bad!");

                    var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name.ToLower() == country.Name.ToLower());
                    if (existingCountry == null)
                    {
                        var createCountry = new Countrie
                        {
                            Name = country.Name,
                            RegionId = newRegion.Id
                        };
                        _context.Countries.Add(createCountry);
                    }
                    else
                    {
                        var regionHasCountry = newRegion.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
                        if (!regionHasCountry)
                        {
                            existingCountry.RegionId = newRegion.Id;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Ok(Mapper.Map<Region, RegionDto>(newRegion));
        }
    }
}