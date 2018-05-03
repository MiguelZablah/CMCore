﻿using System.Threading.Tasks;
using AutoMapper;
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
            {
                return BadRequest("Tag dosen't exist!");
            }

            return Ok(Mapper.Map<Tag, TagDto>(tagInDb));
        }

        // PATCH tag/id
        [HttpPatch("{id}")]
        public IActionResult Edit(int id, [FromBody] TagDto tagDto)
        {
            var tagInDb = _tagService.Exist(id);
            if (tagInDb == null)
            {
                return BadRequest("Tag dosen't exist!");
            }

            var errorMsg = _tagService.Validate(tagDto);
            if (errorMsg != null)
            {
                return BadRequest(errorMsg);
            }

            var errMsg = _tagService.Compare(tagInDb, tagDto);
            if (errMsg != null)
            {
                return BadRequest(errMsg);
            }

            var tagSave = _tagService.Edit(tagInDb, tagDto);

            return Ok(tagSave);
        }

        // DELETE tag/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tagInDb = _tagService.Exist(id);
            if (tagInDb == null)
            {
                return BadRequest("Tag dosen't exist!");
            }

            var delete = _tagService.Erase(tagInDb);
            if (!delete)
            {
                return BadRequest("Tag not deleted!");
            }

            return Ok("Tag Deleted: " + tagInDb.Name);
        }

        // POST tag/
        [HttpPost]
        public async Task<IActionResult> New([FromBody] TagDto tagDto)
        {
            var errorMsg = _tagService.Validate(tagDto);
            if (errorMsg != null)
            {
                return BadRequest(errorMsg);
            }

            var newTag = await _tagService.SaveNew(tagDto);
            return Ok(newTag);
        }
    }
}