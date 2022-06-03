using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Makale.DataAccsessLayer
{
    public class Repository<T> where T:class //bunu koymadan önce set kızıyordu. DbSet in çalışması için bunu yazmak zorundayız.
    {
        private DatabaseContext db;
        private DbSet<T> _dbset; 
        public Repository()
        {
            db = Singleton.CreateContext();
            _dbset = db.Set<T>(); //her seferinde tekrar oluşmasın diye ctor da tanımladık.
        }
        public IQueryable<T> ListQueryable()
        {
            return _dbset.AsQueryable<T>();
        }

        public List<T> List()
        {
            return _dbset.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> kosul) 
        {
            //Kullanicilar.Where(x => x.Id > 10).Tolist();
            return _dbset.Where(kosul).ToList();
        }

        public int Insert(T nesne)
        {
            _dbset.Add(nesne);
            if (nesne is EntitiesBase)
            {
                EntitiesBase obje = nesne as EntitiesBase;
                obje.ModifiedDate = DateTime.Now;
                obje.CreateDate = DateTime.Now;
                if (obje.ModifiedUsername==null)
                {
                    obje.ModifiedUsername = "system";
                }              
                //miras alan yerlerde heryere aynı seyi yazmamak için buraya tek seferde yazılabilir.
            }
            return Save();
        }

        public int Update(T nesne)
        {
            if (nesne is EntitiesBase)
            {
                EntitiesBase obje = nesne as EntitiesBase;
                obje.ModifiedDate = DateTime.Now;
                if (obje.ModifiedUsername == null)
                    obje.ModifiedUsername = "system";    //miras alan yerlerde heryere aynı seyi yazmamak için buraya tek seferde yazılabilir.
            }
            return Save();
        }

        public int Delete(T nesne)
        {
            _dbset.Remove(nesne);
            return Save();
        }

        private int Save()
        {
            return db.SaveChanges();
        }

        public T Find(Expression<Func<T,bool>> kosul)
        {
            return _dbset.FirstOrDefault(kosul);
        }
    }
}
//Dizayn Pattern: Repository Pattern   
//insert, update, delete, list
//çok fazla pattern var araştır sorarlar mülakatlarda