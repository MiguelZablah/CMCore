using AutoMapper.QueryableExtensions;
using CMCore.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CMCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using File = CMCore.Models.File;

namespace CMCore.Controllers
{
	[Produces("application/json")]
	[Route("File/")]
	[EnableCors("AllowSpecificOrigin")]
	public class FileController : Controller
	{
		private readonly IFileService _fileService;
		private readonly IMapper _mapper;

		public FileController(IFileService fileService, IMapper mapper)
		{
			_mapper = mapper;
			_fileService = fileService;
		}

		// GET File/
		[HttpGet]
		public IActionResult Get(string name = null)
		{
			var files = _fileService.FindAll(name).ProjectTo<FileDto>(_mapper.ConfigurationProvider).ToList();
			if (!files.Any())
				return Ok("No Files");

			return Ok(files);
		}

		// GET File/id
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var fileInDb = _fileService.Exist(id).ProjectTo<FileDto>(_mapper.ConfigurationProvider).FirstOrDefault();
			if (fileInDb == null)
				return BadRequest("File doesn't exist!");

			return Ok(fileInDb);
		}

		//GET File/id/download
		[HttpGet("{id}/download")]
		public async Task<IActionResult> Download(int id)
		{
			var fileInDb = await _fileService.Exist(id).FirstOrDefaultAsync();
			if (fileInDb == null)
				return NotFound();

			if (string.IsNullOrEmpty(fileInDb.PathName))
				return BadRequest("File path name not found");

			var preSignUrl = _fileService.DownloadFile(fileInDb);
			if (string.IsNullOrWhiteSpace(preSignUrl))
				return BadRequest("File can't download right know!");

			return Ok(preSignUrl);
		}

		// PATCH File/id
		[HttpPatch("{id}")]
		public async Task<IActionResult> Edit(int id, [FromBody] FileDto fileDto)
		{
			if (fileDto == null)
				return BadRequest("You send an invalid file");

			var fileInDb = _fileService.Exist(id)
				.Include(t => t.FileTags)
				.Include(cp => cp.FileCompanies)
				.Include(c => c.FileClubs).FirstOrDefault();
			if (fileInDb == null)
				return BadRequest("File doesn't exist!");

			var errorMsg = _fileService.CheckSameName(fileDto.Name);
			if (errorMsg != null)
				return BadRequest(errorMsg);

			// Clear Relationships
			if (!_fileService.ClearRelations(fileInDb))
				return BadRequest("File can't be updated!");

			// Validate and Add Tags
			var tagErrMsg = _fileService.AddTagR(fileInDb, fileDto);
			if (!string.IsNullOrWhiteSpace(tagErrMsg))
				return BadRequest(tagErrMsg);

			// Validate and Add Companies
			var companyErrMsg = _fileService.AddCompanyR(fileInDb, fileDto);
			if (!string.IsNullOrWhiteSpace(companyErrMsg))
				return BadRequest(companyErrMsg);

			// Validate and Add Clubs
			var clubsErrMsg = _fileService.AddClubR(fileInDb, fileDto);
			if (!string.IsNullOrWhiteSpace(clubsErrMsg))
				return BadRequest(clubsErrMsg);

			fileDto = _fileService.Edit(fileInDb, fileDto);

			var saved = await _fileService.SaveEf();
			if (!saved)
				return BadRequest("File can't be save!");

			return Ok(_fileService.Exist(fileDto.Id).ProjectTo<FileDto>(_mapper.ConfigurationProvider).FirstOrDefault());
		}

		// DELETE File/id
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var fileInDb = await _fileService.Exist(id).FirstOrDefaultAsync();
			if (fileInDb == null)
				return BadRequest("File doesn't exist!");

			var deleteFile = _fileService.EraseFile(fileInDb);
			if (!deleteFile)
				return BadRequest("File was not deleted!");

			var deleteRelation = _fileService.Erase(fileInDb);
			if (!deleteRelation)
				return BadRequest("File not deleted on DB!");

			var saved = await _fileService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok("File Deleted: " + fileInDb.Name);
		}

		// POST File/
		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile file, string fileName)
		{
			var validateFileMsg = _fileService.ValidateFile(file, fileName);
			if (!string.IsNullOrWhiteSpace(validateFileMsg))
				return BadRequest(validateFileMsg);

			var newFile = _fileService.CreateNew(file, fileName);
			if (newFile == null)
				return BadRequest("File not created!");

			var saved = await _fileService.SaveEf();
			if (!saved)
				return BadRequest();

			return Ok(Mapper.Map<File, FileDto>(newFile));
		}
	}
}