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
    [Route("tag/")]
    [EnableCors("AllowSpecificOrigin")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET tag/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var tags = _tagService.FindAll(name).ProjectTo<TagDto>().ToList();
            if (!tags.Any())
                return BadRequest("No Tags");

            return Ok(tags);
        }

        // GET tag/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tagInDb = _tagService.Exist(id).ProjectTo<TagDto>().FirstOrDefault();
            if (tagInDb == null)
                return BadRequest("Tag dosen't exist!");

            return Ok(tagInDb);
        }

        // PATCH tag/id
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] TagDto tagDto)
        {
            if (ReferenceEquals(tagDto, default(TagDto)))
                return BadRequest("You send a empty countrie");

            var tagInDb = _tagService.Exist(id).FirstOrDefault();
            if (tagInDb == null)
                return BadRequest("Tag dosen't exist!");

            var errorMsg = _tagService.CheckSameName(tagDto);
            if (!string.IsNullOrWhiteSpace(errorMsg))
                return BadRequest(errorMsg);

            var newTag = _tagService.Edit(tagInDb, tagDto);

            var saved = await _tagService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(_tagService.Exist(newTag.Id).ProjectTo<TagDto>().FirstOrDefault());
        }

        // DELETE tag/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tagInDb = _tagService.Exist(id).FirstOrDefault();
            if (tagInDb == null)
                return BadRequest("Tag dosen't exist!");

            var delete = _tagService.Erase(tagInDb);
            if (!delete)
                return BadRequest("Tag not deleted!");

            var saved = await _tagService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok("Tag Deleted: " + tagInDb.Name);
        }

        // POST tag/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] TagDto tagDto)
        {
            var errorMsg = _tagService.Validate(tagDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            var newTag = _tagService.CreateNew(tagDto);

            var saved = await _tagService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Tag, TagDto>(newTag));
        }
    }
}