using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
		private readonly IMapper _mapper;

		public CountryController(ICountryService countryService, IMapper mapper)
		{
			_countryService = countryService;
			_mapper = mapper;
		}

		// GET country/
		[HttpGet]
		public IActionResult Get(string name = null)
		{
			var countries = _countryService.FindAll(name).ProjectTo<CountryDto>(_mapper.ConfigurationProvider).ToList();
			if (!countries.Any())
				return BadRequest("No Countries");

			return Ok(countries);
		}

		// GET country/id
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var countriesInDb = _countryService.Exist(id).ProjectTo<CountryDto>(_mapper.ConfigurationProvider).FirstOrDefault();
			if (countriesInDb == null)
				return BadRequest("Country doesn't exist!");

			return Ok(countriesInDb);
		}

		// Patch country/id
		[HttpPatch("{id}")]
		public async Task<IActionResult> Edit(int id, [FromBody] CountryDto countryDto)
		{
			if (countryDto == null)
				return BadRequest("You send a empty country");

			var countryInDb = _countryService.Exist(id).FirstOrDefault();
			if (countryInDb == null)
				return BadRequest("Country doesn't exist!");

			var errorMsg = _countryService.CheckSameName(countryDto.Name);
			if (errorMsg != null)
				return BadRequest(errorMsg);

			var newCountry = _countryService.Edit(countryInDb, countryDto);

			var saved = await _countryService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok(_countryService.Exist(newCountry.Id).ProjectTo<CountryDto>(_mapper.ConfigurationProvider).FirstOrDefault());
		}

		// Delete country/id
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var countryInDb = _countryService.Exist(id).FirstOrDefault();
			if (countryInDb == null)
				return BadRequest("Country doesn't exist!");

			var delete = _countryService.Erase(countryInDb);
			if (!delete)
				return BadRequest("Country not deleted!");

			var saved = await _countryService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok("Country Deleted: " + countryInDb.Name);
		}

		// Post country/
		[HttpPost]
		public async Task<IActionResult> New([FromBody] CountryDto countryDto)
		{
			var errorMsg = _countryService.Validate(countryDto);
			if (errorMsg != null)
				return BadRequest(errorMsg);

			var newCountry = _countryService.CreateNew(countryDto);

			var saved = await _countryService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok(Mapper.Map<Country, CountryDto>(newCountry));
		}
	}
}