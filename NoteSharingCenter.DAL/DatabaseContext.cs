using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteSharingCenter.Entity;

namespace NoteSharingCenter.DAL
{
    public class DatabaseContext:DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Liked> Likes { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new FakeDataCreation());
        }
    }
}
