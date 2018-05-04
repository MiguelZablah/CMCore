using System;
using System.Threading.Tasks;
using CMCore.Data;

namespace CMCore.Services
{
    public interface IEfService
    {
        Task<bool> SaveEf();
    }
    
    public class EfService : IEfService
    {
        private readonly ContentManagerDbContext _context;

        public EfService(ContentManagerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveEf()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
