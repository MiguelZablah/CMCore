using System.Linq;
using AutoMapper;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Interfaces;
using CMCore.Models;
using CMCore.Models.RelacionClass;

namespace CMCore.Services
{
    public class CompanieService : GenericService<Companie, CompanieDto>, ICompanieService
    {
        public CompanieService(ContentManagerDbContext context) : base(context)
        {
        }

        public Companie CreateNew(CompanieDto companieDto)
        {
            var newCompanie = new Companie
            {
                Name = companieDto.Name
            };
            return AddEf(newCompanie) ? newCompanie : default(Companie);
        }

        public string AddFileR(CompanieDto companieDto, File fileInDb)
        {
            var companieInDb = ExistName(companieDto.Name).FirstOrDefault();
            if (companieInDb == null)
            {
                var createdCompanie = new Companie
                {
                    Name = companieDto.Name
                };
                AddEf(createdCompanie);

                var newCompanie = createdCompanie;
                var newFileCompanie = new FileCompanie
                {
                    FileId = fileInDb.Id,
                    CompanieId = newCompanie.Id
                };
                Context.FileCompanies.Add(newFileCompanie);

                return null;
            }

            if (string.IsNullOrEmpty(companieDto.Name))
                return "You send a null or empty Companie!";

            var typeHasClub = fileInDb.FileCompanies.Any(cr => cr.CompanieId== companieInDb.Id);
            if (!typeHasClub)
            {
                var newFileCompanie = new FileCompanie
                {
                    FileId = fileInDb.Id,
                    CompanieId = companieInDb.Id
                };
                Context.FileCompanies.Add(newFileCompanie);
            }

            return null;
        }

    }
}
