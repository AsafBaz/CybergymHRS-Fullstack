using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CybergymHRS.Infrastructure.Data
{
    public class CybergymHrsContextFactory : IDesignTimeDbContextFactory<CybergymHrsContext>
    {
        public CybergymHrsContext CreateDbContext(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<CybergymHrsContext>();
            optionsBuilder.UseSqlServer("Server = host.docker.internal,1433;Database=CybergymHRS;User Id = sa; Password=0ne$trongP4ssword!;TrustServerCertificate=True;");

            return new CybergymHrsContext(optionsBuilder.Options);
        }
    }
}
