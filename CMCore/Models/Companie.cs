using CMCore.Models.RelacionClass;
using System.Collections.Generic;
using CMCore.Interfaces;

namespace CMCore.Models
{
    public class Companie : IEntity
    {
        public Companie()
        {
            FileCompanies = new List<FileCompanie>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public IList<FileCompanie> FileCompanies { get; set; }
    }
}
