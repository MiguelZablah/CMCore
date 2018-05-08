using System.Collections.Generic;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface IClubService
    {
        List<ClubDto> FindAll(string name);
        Club Exist(int id);
        Club ExistName(string name);
        ClubDto Edit(Club clubInDb, ClubDto clubDto);
        string Validate(ClubDto clubDto);
        string CheckSameName(ClubDto clubDto);
        string Compare(Club clubInDb, ClubDto clubDto);
        Club CreateNew(ClubDto clubDto);
        bool Erase(Club clubInDb);
    }
}
