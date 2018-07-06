using System.Linq;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMCore.Interfaces
{
    public interface IFileService
    {
        IQueryable<File> FindAll(string name);
        IQueryable<File> Exist(int id);
        IQueryable<File> ExistName(string name);
        string ValidateFile(IFormFile file, string fileName);
        string CheckSameName(string name);
        bool EraseFile(File fileInDb);
        bool Erase(File fileInDb);
        Task<bool> SaveEf();
        FileDto Edit(File fileInDb, FileDto fileDto);
        File CreateNew(IFormFile file, string fileName);
        string GetFilePath(File fileInDb);
        Task<IActionResult> DowloadFile(File fileInDb, Controller controller);
        string AddTagR(File fileInDb, FileDto fileDto);
        string AddCompanieR(File fileInDb, FileDto fileDto);
        string AddClubR(File fileInDb, FileDto fileDto);
    }
}
