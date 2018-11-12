using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Cities> cities {get; set;}
        public DbSet<PointOfInterest> poi { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
