using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.DataAccsessLayer
{
    public class DatabaseContext:DbContext
    {
        public DbSet<Kullanici> Kullanıcılar { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Liked> Likes { get; set; }
     
        public DatabaseContext()
        {
            Database.SetInitializer(new DBInitialize());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)  //FluentAPI  //bu üçüncü yol bunu yazdığımda database yeniden oluşturduğumda cascade olarak gelecek "codefirst" te kullanılır.
        {
            modelBuilder.Entity<Note>().HasMany(n => n.Comments).WithRequired(c => c.Note).WillCascadeOnDelete(true);
            modelBuilder.Entity<Note>().HasMany(n => n.Likes).WithRequired(c => c.Note).WillCascadeOnDelete(true);
        }
    }
}
