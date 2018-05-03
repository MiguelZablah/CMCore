using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("country/")]
    [EnableCors("AllowSpecificOrigin")]
    public class CountryController : Controller
    {
        private readonly ContentManagerDbContext _context;

        public CountryController(ContentManagerDbContext context)
        {
            _context = context;
        }

        // GET country/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var contryQuery = _context.Countries.ProjectTo<CountrieDto>();

            if (!String.IsNullOrWhiteSpace(name))
                contryQuery = contryQuery.Where(f => f.Name.ToLower().Contains(name));

            var countries = contryQuery.ToList();

            if (countries.Count <= 0)
                return BadRequest("No Countries");

            return Ok(countries);
        }

        // GET country/
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var country = _context.Countries.ProjectTo<CountrieDto>().SingleOrDefault(c => c.Id == id);

            if (country == null)
                return BadRequest("Country not found");

            return Ok(country);
        }

        // Patch country/id
        [HttpPatch("{id}")]
        public IActionResult Edit(int id, [FromBody] CountrieDto countryDto)
        {
            if (countryDto == null)
                return BadRequest("Did you realy send a Country?");

            var countryInDb = _context.Countries.SingleOrDefault(c => c.Id == id);

            if (countryInDb == null)
                return NotFound();

            if (!String.IsNullOrEmpty(countryDto.Name))
            {
                if (countryInDb.Name.ToLower() == countryDto.Name.ToLower())
                    return BadRequest("Same name, not changes made");

                if (_context.Tags.Any(t => t.Name.ToLower() == countryDto.Name.ToLower()))
                    return BadRequest("A Country with that name already exist!");
            }

            if (String.IsNullOrEmpty(countryDto.Name))
                return BadRequest("You send a null or empty string!");

            Mapper.Map(countryDto, countryInDb);

            _context.SaveChanges();

            // Return new file
            var country = _context.Countries.ProjectTo<CountrieDto>().SingleOrDefault(f => f.Id == countryInDb.Id);

            return Ok(country);
        }

        // Delete country/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var countryInDb = _context.Countries.SingleOrDefault(c => c.Id == id);

            if (countryInDb == null)
                return NotFound();

            _context.Countries.Remove(countryInDb);
            _context.SaveChanges();

            return Ok("Country Deleted: " + id);
        }

        // Post country/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] CountrieDto countryDto)
        {
            if (countryDto == null)
                return BadRequest("Did you send one Country or somthing else?!");

            if (String.IsNullOrEmpty(countryDto.Name))
                return BadRequest("Not Country name send!");

            if (_context.Countries.Any(t => t.Name.ToLower() == countryDto.Name.ToLower()))
                return BadRequest("Country name already exist! No duplicates plz!");

            var newCountry = new Countrie
            {
                Name = countryDto.Name
            };
            _context.Countries.Add(newCountry);
            await _context.SaveChangesAsync();
            return Ok(Mapper.Map<Countrie, CountrieDto>(newCountry));
        }
    }
}