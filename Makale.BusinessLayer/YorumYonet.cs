using Makale.DataAccsessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class YorumYonet
    {
        Repository<Comment> repo_yorum = new Repository<Comment>();
       
        public int YorumSil(Comment yorum)
        {
            Comment c = repo_yorum.Find(x => x.ID == yorum.ID);
            int sonuc = 0;
            if (c != null)
            {
                sonuc = repo_yorum.Delete(c);
                return sonuc;
            }
            return sonuc;
           
        }

        public Comment YorumBul(int id)
        {
            return repo_yorum.Find(x => x.ID == id);
        }

        public int YorumUpdate(Comment model)
        {
            int sonuc = repo_yorum.Update(model);

            return sonuc;
        }

        public int YorumKaydet(Comment model)
        {
            int sonuc = repo_yorum.Insert(model);

            return sonuc;
        }
    }
}
