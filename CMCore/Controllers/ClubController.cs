using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelacionClass;
using CMCore.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Type = CMCore.Models.Type;

namespace CMCore.Controllers
{
    [Produces("application/json")]
    [Route("club/")]
    [EnableCors("AllowSpecificOrigin")]
    public class ClubController : Controller
    {

        private readonly IClubService _clubService;
        private readonly IEfService _efService;

        public ClubController(IClubService clubService, IEfService efService)
        {
            _clubService = clubService;
            _efService = efService;
        }

        // GET club/
        [HttpGet]
        public IActionResult Get(string name = null)
        {
            var club = _clubService.FindAll(name);
            if (club == null)
                return BadRequest("No Clubs");

            return Ok(club);
        }

        // GET club/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var clubInDb = _clubService.Exist(id);
            if (clubInDb == null)
                return BadRequest("Club dosen't exist!");

            return Ok(Mapper.Map<Club, ClubDto>(clubInDb));
        }

        // PATCH club/id
        //    [HttpPatch("{id}")]
        //    public IActionResult Edit(int id, [FromBody] ClubDto clubDto)
        //    {
        //        if (clubDto == null)
        //            return BadRequest("Did you really send a club?");

        //        var clubInDb = _clubService.Exist(id);
        //        if (clubInDb == null)
        //            return BadRequest("Club dosen't exist!");

        //        var errorMsg = _clubService.CheckSameName(clubDto);
        //        if (errorMsg != null)
        //            return BadRequest(errorMsg);

        //        if (!string.IsNullOrEmpty(clubDto.Name))
        //        {
        //            if (clubInDb.Name.ToLower() == clubDto.Name.ToLower())
        //                return BadRequest("Same name, not changes made");

        //            if (_context.Clubs.Any(c => c.Name.ToLower() == clubDto.Name.ToLower()))
        //                return BadRequest("A club with this name already exist!");
        //        }

        //        if (!string.IsNullOrEmpty(clubDto.Url))
        //        {
        //            if (clubInDb.Url.ToLower() == clubDto.Url.ToLower())
        //                return BadRequest("Same Url, no changes made!");

        //            if (clubDto.Url != null && _context.Clubs.Any(c => c.Url.ToLower() == clubDto.Url.ToLower()))
        //                return BadRequest("A club with this url already exist!");
        //        }

        //        // Keep name if not send
        //        if (String.IsNullOrEmpty(clubDto.Name))
        //            clubDto.Name = clubInDb.Name;

        //        // Keep url if not send
        //        if (String.IsNullOrEmpty(clubDto.Url))
        //            clubDto.Url = clubInDb.Url;

        //        // Check Types
        //        if (clubDto.Types != null)
        //        {
        //            // Validates Types
        //            foreach (var clubType in clubDto.Types)
        //            {
        //                var existType = _context.Types.SingleOrDefault(et => et.Name.ToLower() == clubType.Name.ToLower());
        //                if (existType == null)
        //                {
        //                    var createdType = new Type
        //                    {
        //                        Name = clubType.Name
        //                    };
        //                    _context.Types.Add(createdType);
        //                    var newType = createdType;

        //                    var newClubTypeRelation = new ClubType
        //                    {
        //                        ClubId = clubInDb.Id,
        //                        TypeId = newType.Id
        //                    };
        //                    _context.ClubTypes.Add(newClubTypeRelation);
        //                }
        //                else
        //                {
        //                    var clubHasType = clubInDb.ClubTypes.Any(ct => ct.TypeId == existType.Id);
        //                    if (clubHasType)
        //                    {
        //                        var newClubTypeRelation = new ClubType
        //                        {
        //                            ClubId = clubInDb.Id,
        //                            TypeId = existType.Id
        //                        };
        //                        _context.ClubTypes.Add(newClubTypeRelation);
        //                    }
        //                }
        //            }
        //        }

        //        // Check Regions
        //        if (clubDto.Regions != null)
        //        {
        //            // Validates Regions
        //            foreach (var clubRegion in clubDto.Regions)
        //            {
        //                var existRegion = _context.Regions.SingleOrDefault(er => er.Name.ToLower() == clubRegion.Name.ToLower());
        //                if (existRegion == null)
        //                {
        //                    var createdRegion = new Region
        //                    {
        //                        Name = clubRegion.Name
        //                    };
        //                    _context.Regions.Add(createdRegion);
        //                    var newRegion = createdRegion;

        //                    var newClubRegionRelation = new ClubRegion
        //                    {
        //                        ClubId = clubInDb.Id,
        //                        RegionId = newRegion.Id
        //                    };
        //                    _context.ClubRegions.Add(newClubRegionRelation);
        //                    // Countrie check
        //                    if (clubRegion.Countries != null)
        //                    {
        //                        foreach (var regionCountry in clubRegion.Countries)
        //                        {
        //                            var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name.ToLower() == regionCountry.Name.ToLower());
        //                            if (existingCountry == null)
        //                            {
        //                                var createCountry = new Countrie
        //                                {
        //                                    Name = regionCountry.Name,
        //                                    RegionId = newRegion.Id
        //                                };
        //                                _context.Countries.Add(createCountry);
        //                            }
        //                            else
        //                            {
        //                                var regionC = clubInDb.ClubRegions.SingleOrDefault(ct => ct.RegionId == newRegion.Id);
        //                                if (regionC != null)
        //                                {
        //                                    var regionHasCountry =
        //                                        regionC.Region.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
        //                                    if (regionHasCountry)
        //                                    {
        //                                        existingCountry.RegionId = newRegion.Id;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    var clubHasRegion = clubInDb.ClubRegions.Any(ct => ct.RegionId == existRegion.Id);
        //                    if (clubHasRegion)
        //                    {
        //                        var newClubRegionRelation = new ClubRegion
        //                        {
        //                            ClubId = clubInDb.Id,
        //                            RegionId = existRegion.Id
        //                        };
        //                        _context.ClubRegions.Add(newClubRegionRelation);
        //                    }
        //                    // Countrie check
        //                    if (clubRegion.Countries != null)
        //                    {
        //                        foreach (var regionCountry in clubRegion.Countries)
        //                        {
        //                            var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name.ToLower() == regionCountry.Name.ToLower());
        //                            if (existingCountry == null)
        //                            {
        //                                var createCountry = new Countrie
        //                                {
        //                                    Name = regionCountry.Name,
        //                                    RegionId = existRegion.Id
        //                                };
        //                                _context.Countries.Add(createCountry);
        //                            }
        //                            else
        //                            {
        //                                var regionC = clubInDb.ClubRegions.SingleOrDefault(ct => ct.RegionId == existRegion.Id);
        //                                if (regionC != null)
        //                                {
        //                                    var regionHasCountry = regionC.Region.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
        //                                    if (regionHasCountry)
        //                                    {
        //                                        existingCountry.RegionId = existRegion.Id;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        Mapper.Map(clubDto, clubInDb);

        //        _context.SaveChanges();

        //        // Return new file
        //        var club = _context.Clubs.ProjectTo<ClubDto>().SingleOrDefault(f => f.Id == clubInDb.Id);

        //        return Ok(club);
        //    }

        //    // Delete club/id
        //    [HttpDelete("{id}")]
        //    public IActionResult Delete(int id)
        //    {
        //        var clubInDb = _context.Clubs.SingleOrDefault(c => c.Id == id);

        //        if (clubInDb == null)
        //            return NotFound();

        //        _context.Clubs.Remove(clubInDb);
        //        _context.SaveChanges();

        //        return Ok("Club Deleted: " + id);
        //    }

        //    // POST club/
        //    [HttpPost]
        //    public async Task<IActionResult> New([FromBody] ClubDto clubDto)
        //    {
        //        if (clubDto == null)
        //            return BadRequest("Did you send one club or somthing else?!");

        //        if (String.IsNullOrEmpty(clubDto.Name))
        //            return BadRequest("Not Club name send!");

        //        if (String.IsNullOrEmpty(clubDto.Url))
        //            return BadRequest("Not Club url send and you need that!");

        //        if (_context.Clubs.Any(c => c.Name.ToLower() == clubDto.Name.ToLower() || c.Url.ToLower() == clubDto.Url.ToLower()))
        //            return BadRequest("A club with this name or url already exist!");

        //        var createClub = new Club
        //        {
        //            Name = clubDto.Name,
        //            Url = clubDto.Url
        //        };
        //        _context.Clubs.Add(createClub);
        //        var newClub = createClub;

        //        // Check Types
        //        if (clubDto.Types != null)
        //        {
        //            // Validates Types
        //            foreach (var clubType in clubDto.Types)
        //            {
        //                var existType = _context.Types.SingleOrDefault(et => et.Name.ToLower() == clubType.Name.ToLower());
        //                if (existType == null)
        //                {
        //                    var createdType = new Type
        //                    {
        //                        Name = clubType.Name
        //                    };
        //                    _context.Types.Add(createdType);
        //                    var newType = createdType;

        //                    var newClubTypeRelation = new ClubType
        //                    {
        //                        ClubId = newClub.Id,
        //                        TypeId = newType.Id
        //                    };
        //                    _context.ClubTypes.Add(newClubTypeRelation);
        //                }
        //                else
        //                {
        //                    var clubHasType = newClub.ClubTypes.Any(ct => ct.TypeId == existType.Id);
        //                    if (!clubHasType)
        //                    {
        //                        var newClubTypeRelation = new ClubType
        //                        {
        //                            ClubId = newClub.Id,
        //                            TypeId = existType.Id
        //                        };
        //                        _context.ClubTypes.Add(newClubTypeRelation);
        //                    }
        //                }
        //            }
        //        }

        //        // Check Regions
        //        if (clubDto.Regions != null)
        //        {
        //            // Validates Regions
        //            foreach (var clubRegion in clubDto.Regions)
        //            {
        //                var existRegion = _context.Regions.SingleOrDefault(er => er.Name.ToLower() == clubRegion.Name.ToLower());
        //                if (existRegion == null)
        //                {
        //                    var createdRegion = new Region
        //                    {
        //                        Name = clubRegion.Name
        //                    };
        //                    _context.Regions.Add(createdRegion);
        //                    var newRegion = createdRegion;

        //                    var newClubRegionRelation = new ClubRegion
        //                    {
        //                        ClubId = newClub.Id,
        //                        RegionId = newRegion.Id
        //                    };
        //                    _context.ClubRegions.Add(newClubRegionRelation);
        //                    // Countrie check
        //                    if (clubRegion.Countries != null)
        //                    {
        //                        foreach (var regionCountry in clubRegion.Countries)
        //                        {
        //                            var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name.ToLower() == regionCountry.Name.ToLower());
        //                            if (existingCountry == null)
        //                            {
        //                                var createCountry = new Countrie
        //                                {
        //                                    Name = regionCountry.Name,
        //                                    RegionId = newRegion.Id
        //                                };
        //                                _context.Countries.Add(createCountry);
        //                            }
        //                            else
        //                            {
        //                                var regionC = newClub.ClubRegions.SingleOrDefault(ct => ct.RegionId == newRegion.Id);
        //                                if (regionC != null)
        //                                {
        //                                    var regionHasCountry =
        //                                        regionC.Region.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
        //                                    if (!regionHasCountry)
        //                                    {
        //                                        existingCountry.RegionId = newRegion.Id;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    var clubHasRegion = newClub.ClubRegions.Any(ct => ct.RegionId == existRegion.Id);
        //                    if (!clubHasRegion)
        //                    {
        //                        var newClubRegionRelation = new ClubRegion
        //                        {
        //                            ClubId = newClub.Id,
        //                            RegionId = existRegion.Id
        //                        };
        //                        _context.ClubRegions.Add(newClubRegionRelation);
        //                    }
        //                    // Countrie check
        //                    if (clubRegion.Countries != null)
        //                    {
        //                        foreach (var regionCountry in clubRegion.Countries)
        //                        {
        //                            var existingCountry = _context.Countries.SingleOrDefault(ec => ec.Name.ToLower() == regionCountry.Name.ToLower());
        //                            if (existingCountry == null)
        //                            {
        //                                var createCountry = new Countrie
        //                                {
        //                                    Name = regionCountry.Name,
        //                                    RegionId = existRegion.Id
        //                                };
        //                                _context.Countries.Add(createCountry);
        //                            }
        //                            else
        //                            {
        //                                var regionC = newClub.ClubRegions.SingleOrDefault(ct => ct.RegionId == existRegion.Id);
        //                                if (regionC != null)
        //                                {
        //                                    var regionHasCountry = regionC.Region.Countries.Any(cr => cr.RegionId == existingCountry.RegionId);
        //                                    if (!regionHasCountry)
        //                                    {
        //                                        existingCountry.RegionId = existRegion.Id;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //        return Ok(Mapper.Map<Club, ClubDto>(newClub));
        //    }
    }
}