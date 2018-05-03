using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
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
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        // GET country/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var countries = _countryService.FindAll(name);

            if (countries == null)
                return BadRequest("No Countries");

            return Ok(countries);
        }

        // GET country/
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var countrieInDb = _countryService.Exist(id);
            if (countrieInDb == null)
            {
                return BadRequest("Country dosen't exist!");
            }

            return Ok(Mapper.Map<Countrie, CountrieDto>(countrieInDb));
        }

        // Patch country/id
        [HttpPatch("{id}")]
        public IActionResult Edit(int id, [FromBody] CountrieDto countrieDto)
        {
            var countrieInDb = _countryService.Exist(id);
            if (countrieInDb == null)
            {
                return BadRequest("Country dosen't exist!");
            }

            var errorMsg = _countryService.Validate(countrieDto);
            if (errorMsg != null)
            {
                return BadRequest(errorMsg);
            }

            var errMsg = _countryService.Compare(countrieInDb, countrieDto);
            if (errMsg != null)
            {
                return BadRequest(errMsg);
            }

            var countrieSave = _countryService.Edit(countrieInDb, countrieDto);

            return Ok(countrieSave);
        }

        // Delete country/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var countrieInDb = _countryService.Exist(id);
            if (countrieInDb == null)
            {
                return BadRequest("Country dosen't exist!");
            }

            var delete = _countryService.Erase(countrieInDb);
            if (!delete)
            {
                return BadRequest("Country not deleted!");
            }

            return Ok("Country Deleted: " + countrieInDb.Name);
        }

        // Post country/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] CountrieDto countrieDto)
        {
            var errorMsg = _countryService.Validate(countrieDto);
            if (errorMsg != null)
            {
                return BadRequest(errorMsg);
            }

            var countrieNew = await _countryService.SaveNew(countrieDto);
            return Ok(countrieNew);
        }
    }
}