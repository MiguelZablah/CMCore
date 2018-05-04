using System.Threading.Tasks;
using AutoMapper;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Services;
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
        private readonly IEfService _efService;

        public TagController(ITagService tagService, IEfService efService)
        {
            _tagService = tagService;
            _efService = efService;
        }

        // GET tag/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var tags = _tagService.FindAll(name);
            if (tags == null)
                return BadRequest("No Tags");

            return Ok(tags);
        }

        // GET tag/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tagInDb = _tagService.Exist(id);
            if (tagInDb == null)
                return BadRequest("Tag dosen't exist!");

            return Ok(Mapper.Map<Tag, TagDto>(tagInDb));
        }

        // PATCH tag/id
        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] TagDto tagDto)
        {
            if (tagDto == null)
                return BadRequest("You send a empty countrie");

            var tagInDb = _tagService.Exist(id);
            if (tagInDb == null)
                return BadRequest("Tag dosen't exist!");

            var errorMsg = _tagService.CheckSameName(tagDto);
            if (errorMsg != null)
                return BadRequest(errorMsg);

            var newTag = _tagService.Edit(tagInDb, tagDto);

            var saved = await _efService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Tag, TagDto>(_tagService.Exist(newTag.Id)));
        }

        // DELETE tag/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tagInDb = _tagService.Exist(id);
            if (tagInDb == null)
                return BadRequest("Tag dosen't exist!");

            var delete = _tagService.Erase(tagInDb);
            if (!delete)
                return BadRequest("Tag not deleted!");

            var saved = await _efService.SaveEf();
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

            var saved = await _efService.SaveEf();
            if (!saved)
                return BadRequest();

            return Ok(Mapper.Map<Tag, TagDto>(newTag));
        }
    }
}