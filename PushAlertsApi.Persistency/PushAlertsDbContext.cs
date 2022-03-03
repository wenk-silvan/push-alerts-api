using Microsoft.EntityFrameworkCore;

namespace PushAlertsApi.Persistency
{
    public class PushAlertsDbContext : DbContext
    {
        public PushAlertsDbContext(DbContextOptions<PushAlertsDbContext> options) : base(options)
        {
            // TODO: Verify if constructor is needed.
        }
    }
}