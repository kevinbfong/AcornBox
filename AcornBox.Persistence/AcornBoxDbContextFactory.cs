using Microsoft.EntityFrameworkCore;

namespace AcornBox.Persistence
{
    public class AcornBoxDbContextFactory : DesignTimeDbContextFactoryBase<AcornBoxDbContext>
    {
        protected override AcornBoxDbContext CreateNewInstance(DbContextOptions<AcornBoxDbContext> options)
        {
            return new AcornBoxDbContext(options);
        }
    }
}