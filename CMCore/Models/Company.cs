using System.Collections.Generic;
using CMCore.Interfaces;
using CMCore.Models.RelationModel;

namespace CMCore.Models
{
    public class Company : IEntity
    {
        public Company()
        {
            FileCompanies = new List<FileCompany>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<FileCompany> FileCompanies { get; set; }
    }
}
