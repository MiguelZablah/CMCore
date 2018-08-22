using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using Lagersoft.OAuth;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CMCore.Controllers
{
	[Produces("application/json")]
	[Route("country/")]
	[EnableCors("AllowSpecificOrigin")]
	[OAuthorize]
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
			var countries = _countryService.FindAll(name).ProjectTo<CountrieDto>().ToList();
			if (!countries.Any())
				return BadRequest("No Countries");

			return Ok(countries);
		}

		// GET country/id
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var countrieInDb = _countryService.Exist(id).ProjectTo<CountrieDto>().FirstOrDefault();
			if (countrieInDb == null)
				return BadRequest("Country dosen't exist!");

			return Ok(countrieInDb);
		}

		// Patch country/id
		[HttpPatch("{id}")]
		public async Task<IActionResult> Edit(int id, [FromBody] CountrieDto countrieDto)
		{
			if (countrieDto == null)
				return BadRequest("You send a empty countrie");

			var countrieInDb = _countryService.Exist(id).FirstOrDefault();
			if (countrieInDb == null)
				return BadRequest("Country dosen't exist!");

			var errorMsg = _countryService.CheckSameName(countrieDto.Name);
			if (errorMsg != null)
				return BadRequest(errorMsg);

			var newCountrie = _countryService.Edit(countrieInDb, countrieDto);

			var saved = await _countryService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok(_countryService.Exist(newCountrie.Id).ProjectTo<CountrieDto>().FirstOrDefault());
		}

		// Delete country/id
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var countrieInDb = _countryService.Exist(id).FirstOrDefault();
			if (countrieInDb == null)
				return BadRequest("Country dosen't exist!");

			var delete = _countryService.Erase(countrieInDb);
			if (!delete)
				return BadRequest("Country not deleted!");

			var saved = await _countryService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok("Country Deleted: " + countrieInDb.Name);
		}

		// Post country/
		[HttpPost]
		public async Task<IActionResult> New([FromBody] CountrieDto countrieDto)
		{
			var errorMsg = _countryService.Validate(countrieDto);
			if (errorMsg != null)
				return BadRequest(errorMsg);

			var newCountrie = _countryService.CreateNew(countrieDto);

			var saved = await _countryService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok(Mapper.Map<Countrie, CountrieDto>(newCountrie));
		}
	}
}