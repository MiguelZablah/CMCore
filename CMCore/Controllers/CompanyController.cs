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
	[Route("Company/")]
	[EnableCors("AllowSpecificOrigin")]
	public class CompanyController : Controller
	{
		private readonly ICompanyService _companyService;
		private readonly IMapper _mapper;

		public CompanyController(ICompanyService companyService, IMapper mapper)
		{
			_companyService = companyService;
			_mapper = mapper;
		}

		// GET company/
		[HttpGet]
		public IActionResult Get(string name = null)
		{
			var companies = _companyService.FindAll(name).ProjectTo<CompanyDto>(_mapper.ConfigurationProvider).ToList();

			if (!companies.Any())
				return BadRequest("No Companies");

			return Ok(companies);
		}

		// GET company/id
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var companyInDb = _companyService.Exist(id).ProjectTo<CompanyDto>(_mapper.ConfigurationProvider).FirstOrDefault();
			if (companyInDb == null)
				return BadRequest("Company doesn't exist!");

			return Ok(companyInDb);
		}

		// PATCH company/id
		[HttpPatch("{id}")]
		public async Task<IActionResult> Edit(int id, [FromBody] CompanyDto companyDto)
		{
			if (companyDto == null)
				return BadRequest("You send a empty company");

			var companyInDb = _companyService.Exist(id).FirstOrDefault();
			if (companyInDb == null)
				return BadRequest("Company doesn't exist!");

			var errorMsg = _companyService.CheckSameName(companyDto.Name);
			if (errorMsg != null)
				return BadRequest(errorMsg);

			var newCompany = _companyService.Edit(companyInDb, companyDto);

			var saved = await _companyService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok(_companyService.Exist(newCompany.Id).ProjectTo<CompanyDto>(_mapper.ConfigurationProvider).FirstOrDefault());
		}

		// DELETE company/id
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var companyInDb = _companyService.Exist(id).FirstOrDefault();
			if (companyInDb == null)
				return BadRequest("Company doesn't exist!");

			var delete = _companyService.Erase(companyInDb);
			if (!delete)
				return BadRequest("Company not deleted!");

			var saved = await _companyService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok("Company Deleted: " + companyInDb.Name);
		}

		// POST company/
		[HttpPost]
		public async Task<IActionResult> New([FromBody] CompanyDto companyDto)
		{
			var errorMsg = _companyService.Validate(companyDto);
			if (errorMsg != null)
				return BadRequest(errorMsg);

			var newCompany = _companyService.CreateNew(companyDto);

			var saved = await _companyService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok(Mapper.Map<Company, CompanyDto>(newCompany));
		}
	}
}