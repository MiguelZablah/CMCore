using System.Linq;
using System.Threading.Tasks;
using CMCore.DTO;
using CMCore.Models;

namespace CMCore.Interfaces
{
    public interface ICompanyService
    {
        IQueryable<Company> FindAll(string name);
        IQueryable<Company> Exist(int id);
        string Validate(CompanyDto companyDto);
        string CheckSameName(string name);
        bool Erase(Company companyInDb);
        Task<bool> SaveEf();
        CompanyDto Edit(Company companyInDb, CompanyDto companyDto);
        Company CreateNew(CompanyDto companyDto);
        string AddFileR(CompanyDto companyDto, File fileInDb);
    }
}
