using Makale.DataAccsessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class KategoriYonet
    {
        private Repository<Kategori> repo_kat = new Repository<Kategori>();

        public List<Kategori> KategoriListesi()
        {
            return repo_kat.List();
        }
        public Kategori KategoriBul(int id)
        {
            return repo_kat.Find(x => x.ID == id);
        }
        BusinessLayerResult<Kategori> result = new BusinessLayerResult<Kategori>();

        public BusinessLayerResult<Kategori> KategoriKaydet(Kategori model)
        {

            result.Nesne = repo_kat.Find(x => x.Title == model.Title);

            if (result.Nesne != null)
            {
                result.hata.Add("Bu kategori kayıtlı");
            }
            else
            {
                int sonuc = repo_kat.Insert(new Kategori()
                {
                   Title=model.Title,
                   Description=model.Description
                });
            }
            return result;
        }

        public BusinessLayerResult<Kategori> KategoriUpdate(Kategori model)
        {
            Kategori kat = repo_kat.Find(x => x.Title == model.Title && x.ID != model.ID);

            if (kat != null)
            {
                result.hata.Add("Bu kategori kayıtlı");
            }
            else
            {
                result.Nesne = repo_kat.Find(x => x.ID == model.ID);
                result.Nesne.Title = model.Title;
                result.Nesne.Description = model.Description;

                int sonuc = repo_kat.Update(result.Nesne);
                if (sonuc>0)
                {
                    result.Nesne = repo_kat.Find(x => x.ID == model.ID);
                }
            }
            return result;
        }
        public BusinessLayerResult<Kategori> KategoriSil(int id)
        {
            //Kategori ile ilişkili notlar silinecek.
            Kategori k = repo_kat.Find(x => x.ID == id);
            NoteYonet ny = new NoteYonet();
            YorumYonet yy = new YorumYonet();
            LikeYonet ly = new LikeYonet();
            //foreach (var not in k.Notes.ToList()) //k nın tüm notlarını sil.
            //{
            //    foreach (var yorum in not.Comments.ToList())
            //    {
            //        yy.YorumSil(yorum);
            //    }
            //    foreach (var like in not.Likes.ToList())
            //    {
            //        ly.LikeSil(like);
            //    }
            //    ny.NoteSil(not);
            //}      //bu birinci yol ikinci yol database gidip bağlantıların properties inden delete cascade yapmak ücüncü yol ise databasecontextte

            //Notlar ile ilişkili yorumlar ve likelar silinecek. 
            if (k != null)
            {
                int sonuc = repo_kat.Delete(k);
                if (sonuc == 0)
                {
                    result.hata.Add("Kategori silinemedi.");
                    return result;
                }
            }
            else
            {
                result.hata.Add("Kategori bulunamadı");
            }
            return result;
        }


    }
}
