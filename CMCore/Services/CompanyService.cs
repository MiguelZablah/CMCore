using System.Linq;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelationModel;

namespace CMCore.Services
{
    public class CompanyService : GenericService<Company, CompanyDto>, ICompanyService
    {
        public CompanyService(ContentManagerDbContext context) : base(context)
        {
        }

        public Company CreateNew(CompanyDto companyDto)
        {
            var newCompany = new Company
            {
                Name = companyDto.Name
            };
            return AddEf(newCompany) ? newCompany : default(Company);
        }

        public string AddFileR(CompanyDto companyDto, File fileInDb)
        {
            var companyInDb = ExistName(companyDto.Name, true).FirstOrDefault();
            if (companyInDb == null)
            {
                var createdCompany = new Company
                {
                    Name = companyDto.Name
                };
                AddEf(createdCompany);

                var newCompany = createdCompany;
                var newFileCompany = new FileCompany
                {
                    FileId = fileInDb.Id,
                    CompanyId = newCompany.Id
                };
                Context.FileCompanies.AddAsync(newFileCompany);

                return null;
            }

            if (string.IsNullOrEmpty(companyDto.Name))
                return "You send a null or empty Company!";

            var typeHasClub = fileInDb.FileCompanies.Any(cr => cr.CompanyId== companyInDb.Id);
            if (!typeHasClub)
            {
                var newFileCompany = new FileCompany
                {
                    FileId = fileInDb.Id,
                    CompanyId = companyInDb.Id
                };
                Context.FileCompanies.AddAsync(newFileCompany);
            }

            return null;
        }

    }
}
