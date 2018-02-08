using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("File/[action]")]
    public class FileController : Controller
    {
        private ContentManagerDbContext _context;

        public FileController()
        {
            _context = new ContentManagerDbContext();
        }

        // GET file/get
        [HttpGet]
        public IActionResult Get()
        {
            var files = _context.Files.ProjectTo<FileDto>().ToList();

            if (files.Count >= 0)
                return NotFound();

            return Ok(files);
        }

        // GET file/get/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var file = _context.Files.ProjectTo<FileDto>().SingleOrDefault(f => f.Id == id);

            if (file == null)
                return NotFound();

            return Ok(file);
        }

        // Put file/post
        [HttpPost]
        public IActionResult Post(FileDto fileDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var file = Mapper.Map<FileDto, File>(fileDto);
            _context.Files.Add(file);
            _context.SaveChanges();

            fileDto.Id = file.Id;

            return Created("File added to DB", fileDto);
        }

        // Put file/put
        [HttpPut("{id}")]
        public IActionResult Put(int id, FileDto fileDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var fileInDB = _context.Files.SingleOrDefault(c => c.Id == id);

            if (fileInDB == null)
                return NotFound();

            Mapper.Map(fileDto, fileInDB);

            _context.SaveChanges();

            return Ok("File Edited");
        }

        // Delete file/delete/
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var fileInDB = _context.Files.SingleOrDefault(c => c.Id == id);

            if (fileInDB == null)
                return NotFound();

            _context.Files.Remove(fileInDB);
            _context.SaveChanges();

            return Ok("File Deleted");
        }

        //[HttpPost]
        //public JsonResult PostImage(Guid id)
        //{
        //    foreach (string fileName in Request.Files)
        //    {
        //        HttpPostedFileBase file = Request.Files[fileName];
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            ImageService.UploadPendingImage(file, Prefixes.PerformerImage(id.ToString()));
        //        }
        //        performerService.AddPendingImage(performer.Id);
        //    }
        //    return Json(response);
        //}

        //public static void UploadPendingImage(HttpPostedFileBase file, string prefixKey)
        //{
        //    MemoryStream target = new MemoryStream();
        //    file.InputStream.Position = 0;
        //    file.InputStream.CopyTo;
        //    byte[] photoBytes = target.ToArray();
        //    using (MemoryStream inStream = new MemoryStream(photoBytes))
        //    {
        //        var fileName = DateTime.UtcNow.Ticks.ToString();
        //        UploadOriginal(TempCredentials, TempBucketName, inStream, fileName, $"{prefixKey}/images");
        //    }
        //}
    }
}