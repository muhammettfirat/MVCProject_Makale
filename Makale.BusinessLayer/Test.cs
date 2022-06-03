using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makale.Entities;
using Makale.DataAccsessLayer;

namespace Makale.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            //DataAccsessLayer.DatabaseContext db = new DataAccsessLayer.DatabaseContext();
            //db.Database.CreateIfNotExists();

            //db.Kategoriler.ToList();
            Repository<Kategori> repo = new Repository<Kategori>();
            //List<Kategori> liste = repo.List();
            List<Kategori> liste = repo.List(x => x.ID > 3);
        }
        Repository<Kullanici> repo_user = new Repository<Kullanici>();
        Repository<Note> repo_not = new Repository<Note>();
        Repository<Comment> repo_comment = new Repository<Comment>();
        public void InsertTest()
        {
            int sonuc = repo_user.Insert(new Kullanici()
            {
                Name = "test",
                Surname = "test",
                Email = "test@test",
                IsActive = true,
                IsAdmin = true,
                Username = "test",
                Password = "123",
                CreateDate = DateTime.Now,
                ModifiedDate = DateTime.Now.AddMinutes(5),
                ActivateGuid = Guid.NewGuid(),
                ModifiedUsername = "test"

            });
            
        }
        public void UpdateTest()
        {
            Kullanici k = repo_user.Find(x => x.Username == "test");
            k.Username = "xxx";
            int sonuc=repo_user.Update(k);
        }

        public void DeleteTest()
        {
            Kullanici k = repo_user.Find(x => x.Username == "xxx");
            if (k != null)
                repo_user.Delete(k);
        }
        public void CommentTest()
        {
            Kullanici k = repo_user.Find(x => x.ID == 1);
            Note not = repo_not.Find(x => x.ID == 1);
            Comment comment = new Comment()
            {
                Text = "Bu bir test kayıttır.",
                CreateDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ModifiedUsername = "betulerkal",
                Note=not,
                Kullanici=k
                
            };
            repo_comment.Insert(comment);
        }
    }
}
