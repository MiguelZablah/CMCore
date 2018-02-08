using CMCore.Models.RelacionClass;
using System.Collections.Generic;

namespace CMCore.Models
{
    public class Companie
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
