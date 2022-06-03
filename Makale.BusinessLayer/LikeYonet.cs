using Makale.DataAccsessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class LikeYonet
    {
        Repository<Liked> repo_like = new Repository<Liked>();
        BusinessLayerResult<Liked> result = new BusinessLayerResult<Liked>();
        public int LikeSil(Liked like)
        {         
            int sonuc = repo_like.Delete(like);
            return sonuc;
        }
        public int LikeInsert(Liked like)
        {
            int sonuc = repo_like.Insert(like);
            return sonuc;
        }
        public IQueryable<Liked> ListQueryable()
        {
            return repo_like.ListQueryable();
        }

        public Liked LikeBul(int notid,int userid)
        {           
            Liked like = repo_like.Find(x => x.Note.ID ==notid && x.Kullanici.ID==userid);
            return like;
        }

        public List<Liked> List(Expression<Func<Liked, bool>> kosul)
        {
            return repo_like.List(kosul);
        }
    }
}
