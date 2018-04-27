using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMCore.Models.RelacionClass;
using File = CMCore.Models.File;
using Type = CMCore.Models.Type;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("File/[action]")]
    [EnableCors("AllowSpecificOrigin")]
    public class FileController : Controller
    {
        private readonly ContentManagerDbContext _context;
        private readonly IHostingEnvironment _host;
        private readonly FileSettings _fileSettings;

        public FileController(IHostingEnvironment host, ContentManagerDbContext context, IOptionsSnapshot<FileSettings> fileSettings)
        {
            _fileSettings = fileSettings.Value;
            _host = host;
            _context = context;
        }

        // GET file/get
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var filesQuery = _context.Files.ProjectTo<FileDto>();

            if (!String.IsNullOrWhiteSpace(name))
                filesQuery = filesQuery.Where(f => f.Name.ToLower().Contains(name));

            var files = filesQuery.ToList();

            if (files.Count <= 0)
                return BadRequest("No files");

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

        // Put file/edit/id
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] FileDto fileDto)
        {
            if (fileDto == null)
                return BadRequest("Did you send a file info or somthing?!");

            var fileInDb = _context.Files.SingleOrDefault(c => c.Id == id);

            if (fileInDb == null)
                return NotFound();

            if (!String.IsNullOrEmpty(fileDto.Name))
            {
                if (fileInDb.Name.ToLower() == fileDto.Name.ToLower())
                    return BadRequest("Same name, no changes made");

                if (_context.Files.Any(f => f.Name.ToLower() == fileDto.Name.ToLower()))
                    return BadRequest("A file with that name already exist!");
            }

            // Keep pathName and extensions
            fileDto.PathName = fileInDb.PathName;
            fileDto.ExtensionId = fileInDb.ExtensionId;

            // Keep name if not send
            if (String.IsNullOrEmpty(fileDto.Name))
                fileDto.Name = fileInDb.Name;

            // Keep description if not send
            if (String.IsNullOrEmpty(fileDto.Description))
                fileDto.Description = fileInDb.Description;

            // Tags creation or update
            if (fileDto.Tags != null)
            {
                foreach (var tag in fileDto.Tags)
                {
                    var tagExist = _context.Tags.SingleOrDefault(t => t.Name == tag.Name);
                    if (tagExist == null)
                    {
                        var createTag = new Tag
                        {
                            Name = tag.Name
                        };
                        _context.Tags.Add(createTag);
                        var newTag = createTag;

                        var newFileTag = new FileTag
                        {
                            FileId = fileInDb.Id,
                            TagId = newTag.Id
                        };
                        _context.FileTags.Add(newFileTag);
                    }
                    else
                    {
                        var checkRelation = fileInDb.FileTags.Any(ft => ft.TagId == tag.Id);
                        if (!checkRelation) continue;
                        var newFileTag = new FileTag
                        {
                            FileId = fileInDb.Id,
                            TagId = tagExist.Id
                        };
                        _context.FileTags.Add(newFileTag);
                    }

                }
            }

            // Companie creation or update
            if (fileDto.Companies != null)
            {
                foreach (var companie in fileDto.Companies)
                {
                    var companiesExist = _context.Companies.SingleOrDefault(e => e.Name == companie.Name);
                    if (companiesExist == null)
                    {
                        var createCompanie = new Companie
                        {
                            Name = companie.Name
                        };
                        _context.Companies.Add(createCompanie);
                        var newCompanie = createCompanie;

                        var newFileCompanie = new FileCompanie
                        {
                            FileId = fileInDb.Id,
                            CompanieId = newCompanie.Id
                        };
                        _context.FileCompanies.Add(newFileCompanie);
                    }
                    else
                    {
                        var fileHasCompanie = fileInDb.FileCompanies.Any(fDb => fDb.CompanieId == companie.Id);
                        if (!fileHasCompanie) continue;
                        var newFileCompanie = new FileCompanie
                        {
                            FileId = fileInDb.Id,
                            CompanieId = companiesExist.Id
                        };
                        _context.FileCompanies.Add(newFileCompanie);
                    }

                }
            }

            // Check for clubs and childrens
            if (fileDto.Clubs != null)
            {
                foreach (var club in fileDto.Clubs)
                {
                    var clubExist = _context.Clubs.SingleOrDefault(ec => ec.Name == club.Name);
                    if (clubExist == null)
                    {
                        var createdClub = new Club
                        {
                            Name = club.Name,
                            Url = club.Url
                        };
                        _context.Clubs.Add(createdClub);
                        var newClub = createdClub;

                        var newFileClub = new FileClub
                        {
                            FileId = fileInDb.Id,
                            ClubId = newClub.Id
                        };
                        _context.FileClubs.Add(newFileClub);
                        // Validates Types
                        if (club.Types != null)
                        {
                            foreach (var clubType in club.Types)
                            {
                                var existType = _context.Types.SingleOrDefault(et => et.Name == clubType.Name);
                                if (existType == null)
                                {
                                    var createdType = new Type
                                    {
                                        Name = clubType.Name
                                    };
                                    _context.Types.Add(createdType);
                                    var newType = createdType;

                                    var newClubTypeRelation = new ClubType
                                    {
                                        ClubId = newClub.Id,
                                        TypeId = newType.Id
                                    };
                                    _context.ClubTypes.Add(newClubTypeRelation);
                                }
                                else
                                {
                                    var clubHasType =
                                        fileInDb.FileClubs.SingleOrDefault(fc => fc.ClubId == club.Id)
                                            .Club.ClubTypes.Any(ct => ct.TypeId == existType.Id);
                                    if (clubHasType)
                                    {
                                        var newClubTypeRelation = new ClubType
                                        {
                                            ClubId = newClub.Id,
                                            TypeId = existType.Id
                                        };
                                        _context.ClubTypes.Add(newClubTypeRelation);
                                    }
                                }
                            }
                        }
                        
                        // Check Regions
                        if (club.Regions != null)
                        {
                            foreach (var clubRegion in club.Regions)
                            {
                                var existRegion = _context.Regions.SingleOrDefault(er => er.Name == clubRegion.Name);
                                if (existRegion == null)
                                {
                                    var createdRegion = new Region
                                    {
                                        Name = clubRegion.Name
                                    };
                                    _context.Regions.Add(createdRegion);
                                    var newRegion = createdRegion;

                                    var newClubRegionRelation = new ClubRegion
                                    {
                                        ClubId = newClub.Id,
                                        RegionId = newRegion.Id
                                    };
                                    _context.ClubRegions.Add(newClubRegionRelation);
                                    // Countrie check
                                    if (clubRegion.Countries != null)
                                    {
                                        foreach (var regionCountry in clubRegion.Countries)
                                        {
                                            var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name == regionCountry.Name);
                                            if (existingCountry == null)
                                            {
                                                var createCountry = new Countrie
                                                {
                                                    Name = regionCountry.Name,
                                                    RegionId = newRegion.Id
                                                };
                                                _context.Countries.Add(createCountry);
                                            }
                                            else
                                            {
                                                var regionHasCountry = fileInDb.FileClubs
                                                    .SingleOrDefault(fc => fc.ClubId == club.Id)
                                                    .Club.ClubRegions.SingleOrDefault(ct => ct.RegionId == newRegion.Id)
                                                    .Region.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
                                                if (regionHasCountry)
                                                {
                                                    existingCountry.RegionId = newRegion.Id;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var clubHasRegion = fileInDb.FileClubs
                                        .SingleOrDefault(fc => fc.ClubId == club.Id)
                                        .Club.ClubRegions.Any(ct => ct.RegionId == existRegion.Id);
                                    if (clubHasRegion)
                                    {
                                        var newClubRegionRelation = new ClubRegion
                                        {
                                            ClubId = newClub.Id,
                                            RegionId = existRegion.Id
                                        };
                                        _context.ClubRegions.Add(newClubRegionRelation);
                                    }
                                    // Countrie check
                                    if (clubRegion.Countries != null)
                                    {
                                        foreach (var regionCountry in clubRegion.Countries)
                                        {
                                            var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name == regionCountry.Name);
                                            if (existingCountry == null)
                                            {
                                                var createCountry = new Countrie
                                                {
                                                    Name = regionCountry.Name,
                                                    RegionId = existRegion.Id
                                                };
                                                _context.Countries.Add(createCountry);
                                            }
                                            else
                                            {
                                                var regionHasCountry = fileInDb.FileClubs
                                                    .SingleOrDefault(fc => fc.ClubId == club.Id)
                                                    .Club.ClubRegions.SingleOrDefault(ct => ct.RegionId == existRegion.Id)
                                                    .Region.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
                                                if (regionHasCountry)
                                                {
                                                    existingCountry.RegionId = existRegion.Id;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // If Club Exist
                    else
                    {
                        var checkRelation = fileInDb.FileClubs.Any(ft => ft.ClubId == club.Id);
                        if (checkRelation)
                        {
                            var newFileClub = new FileClub
                            {
                                FileId = fileInDb.Id,
                                ClubId = clubExist.Id
                            };
                            _context.FileClubs.Add(newFileClub);
                        }
                        // Change url if was change
                        if(club.Url != null)
                            clubExist.Url = club.Url;

                        // Validates Types
                        if (club.Types != null)
                        {
                            foreach (var clubType in club.Types)
                            {
                                var existType = _context.Types.SingleOrDefault(et => et.Name == clubType.Name);
                                if (existType == null)
                                {
                                    var createdType = new Type
                                    {
                                        Name = clubType.Name
                                    };
                                    _context.Types.Add(createdType);
                                    var newType = createdType;

                                    var newClubTypeRelation = new ClubType
                                    {
                                        ClubId = clubExist.Id,
                                        TypeId = newType.Id
                                    };
                                    _context.ClubTypes.Add(newClubTypeRelation);
                                }
                                else
                                {
                                    var clubHasType =
                                        fileInDb.FileClubs.SingleOrDefault(fc => fc.ClubId == club.Id)
                                            .Club.ClubTypes.Any(ct => ct.TypeId == existType.Id);
                                    if (clubHasType)
                                    {
                                        var newClubTypeRelation = new ClubType
                                        {
                                            ClubId = clubExist.Id,
                                            TypeId = existType.Id
                                        };
                                        _context.ClubTypes.Add(newClubTypeRelation);
                                    }
                                }
                            }
                        }
                        
                        // Check Regions
                        if (club.Regions != null)
                        {
                            foreach (var clubRegion in club.Regions)
                            {
                                var existRegion = _context.Regions.SingleOrDefault(er => er.Name == clubRegion.Name);
                                if (existRegion == null)
                                {
                                    var createdRegion = new Region
                                    {
                                        Name = clubRegion.Name
                                    };
                                    _context.Regions.Add(createdRegion);
                                    var newRegion = createdRegion;

                                    var newClubRegionRelation = new ClubRegion
                                    {
                                        ClubId = clubExist.Id,
                                        RegionId = newRegion.Id
                                    };
                                    _context.ClubRegions.Add(newClubRegionRelation);
                                    // Countrie check
                                    if (clubRegion.Countries != null)
                                    {
                                        foreach (var regionCountry in clubRegion.Countries)
                                        {
                                            var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name == regionCountry.Name);
                                            if (existingCountry == null)
                                            {
                                                var createCountry = new Countrie
                                                {
                                                    Name = regionCountry.Name,
                                                    RegionId = newRegion.Id
                                                };
                                                _context.Countries.Add(createCountry);
                                            }
                                            else
                                            {
                                                var regionHasCountry = fileInDb.FileClubs
                                                    .SingleOrDefault(fc => fc.ClubId == club.Id)
                                                    .Club.ClubRegions.SingleOrDefault(ct => ct.RegionId == newRegion.Id)
                                                    .Region.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
                                                if (regionHasCountry)
                                                {
                                                    existingCountry.RegionId = newRegion.Id;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (_context.ClubRegions.Any(cr => cr.ClubId == clubRegion.Id && cr.ClubId == club.Id))
                                    {
                                        var newClubRegionRelation = new ClubRegion
                                        {
                                            ClubId = clubExist.Id,
                                            RegionId = existRegion.Id
                                        };
                                        _context.ClubRegions.Add(newClubRegionRelation);
                                    }
                                    // Countrie check
                                    if (clubRegion.Countries != null)
                                    {
                                        foreach (var regionCountry in clubRegion.Countries)
                                        {
                                            var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name == regionCountry.Name);
                                            if (existingCountry == null)
                                            {
                                                var createCountry = new Countrie
                                                {
                                                    Name = regionCountry.Name,
                                                    RegionId = existRegion.Id
                                                };
                                                _context.Countries.Add(createCountry);
                                            }
                                            else
                                            {
                                                var regionHasCountry = fileInDb.FileClubs
                                                    .SingleOrDefault(fc => fc.ClubId == club.Id)
                                                    .Club.ClubRegions.SingleOrDefault(ct => ct.RegionId == existRegion.Id)
                                                    .Region.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
                                                if (regionHasCountry)
                                                {
                                                    existingCountry.RegionId = existRegion.Id;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Mapper.Map(fileDto, fileInDb);

            _context.SaveChanges();

            // Return new file
            var file = _context.Files.ProjectTo<FileDto>().SingleOrDefault(f => f.Id == fileInDb.Id);

            return Ok(file);
        }

        //GET file/download/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var fileInDb = await _context.Files.FindAsync(id);

            if (fileInDb == null)
                return NotFound();

            if (String.IsNullOrEmpty(fileInDb.PathName))
                return BadRequest("File path name nor found");

            var filePath = Path.Combine(_host.WebRootPath, "FilesUploads", fileInDb.PathName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            if (String.IsNullOrEmpty(_fileSettings.GetContentType(fileInDb.PathName)))
                return BadRequest("No content Type Found");

            memory.Position = 0;
            return File(memory, _fileSettings.GetContentType(fileInDb.PathName), Path.GetFileName(filePath));
        }

        // Delete file/delete/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fileInDb = await _context.Files.FindAsync(id);

            if (fileInDb == null)
                return NotFound();

            var filePath = Path.Combine(_host.WebRootPath, "FilesUploads", fileInDb.PathName);

            if (!System.IO.File.Exists(filePath))
                return BadRequest("File path dosen't exist!");

            System.IO.File.Delete(filePath);
            _context.Files.Remove(fileInDb);
            await _context.SaveChangesAsync();

            return Ok("File Deleted: " + id);
        }

        // Delete file/Upload/
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return BadRequest("No file name");

            if(_context.Files.Any(f => f.Name.ToLower() == fileName.ToLower()))
                return BadRequest("File name already exist!");

            if (file == null)
                return BadRequest("No file");

            if (file.Length <= 0)
                return BadRequest("Empty file");


            if (!_fileSettings.IsSupported(file.FileName))
                return BadRequest("Invalid File Type");

            var uploadFolderUrl = Path.Combine(_host.WebRootPath, "FilesUploads");

            if (!Directory.Exists(uploadFolderUrl))
                Directory.CreateDirectory(uploadFolderUrl);

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var filePathName = Guid.NewGuid().ToString();
            var filePath = Path.Combine(uploadFolderUrl, filePathName + fileExtension);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var extensionExist = _context.Extensions.SingleOrDefault(e => e.Name == fileExtension);
            if (extensionExist == null)
            {
                var newExtension = new Extension
                {
                    Name = fileExtension
                };
                _context.Extensions.Add(newExtension);
                extensionExist = newExtension;
            }

            var newFile = new File
            {
                Name = fileName,
                PathName = filePathName + fileExtension,
                ExtensionId = extensionExist.Id
            };
            _context.Files.Add(newFile);
            await _context.SaveChangesAsync();

            return Ok(Mapper.Map<File, FileDto>(newFile));
        }
    }
}