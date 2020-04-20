using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        //for services 
        public DataContext(DbContextOptions<DataContext> options):base(options){}
        public DbSet<Value> Values { get; set; }
        
        public DbSet<User> Users { get; set; }
       

    }
}