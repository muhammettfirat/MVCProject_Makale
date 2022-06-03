using Makale.DataAccsessLayer;
using Makale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class NoteYonet
    {
        Repository<Note> repo_not = new Repository<Note>();
        public List<Note> NotListesi()
        {
            return repo_not.List();
        }
        public Note NotBul(int id)
        {
            return repo_not.Find(x => x.ID == id);
        }

        public IQueryable<Note> ListQueryable()
        {
            return repo_not.ListQueryable();
        }

        BusinessLayerResult<Note> result = new BusinessLayerResult<Note>();
        public BusinessLayerResult<Note> NoteSil(Note not)
        {
            Note n = repo_not.Find(x => x.ID == not.ID);
            if (n != null)
            {
                int sonuc = repo_not.Delete(n);
                if (sonuc == 0)
                {
                    result.hata.Add("Notlar silinemedi.");
                    return result;
                }
            }
            else
            {
                result.hata.Add("Not bulunamadı");
            }
            return result;
        }
        public BusinessLayerResult<Note> NoteKaydet(Note model)
        {

            result.Nesne = repo_not.Find(x => x.Title == model.Title && x.KategoriId == model.KategoriId);

            if (result.Nesne != null)
            {
                result.hata.Add("Bu not kayıtlı");
            }
            else
            {
                Note n = new Note();
                n.Kullanici = model.Kullanici;
                n.KategoriId = model.KategoriId;
                n.Text = model.Text;
                n.Title = model.Title;
                n.IsDraft = model.IsDraft;
                n.ModifiedUsername = model.Kullanici.Username;
                int sonuc = repo_not.Insert(n);

            }
            return result;
        }

        public BusinessLayerResult<Note> NoteUpdate(Note model)
        {
            Note not = repo_not.Find(x => x.Title == model.Title && x.ID != model.ID && x.KategoriId == model.KategoriId);

            if (not != null)
            {
                result.hata.Add("Bu not kayıtlı");
            }
            else
            {
                result.Nesne = repo_not.Find(x => x.ID == model.ID);
                result.Nesne.Title = model.Title;
                result.Nesne.Text = model.Text;
                result.Nesne.IsDraft = model.IsDraft;
                result.Nesne.KategoriId = model.KategoriId;
                result.Nesne.ModifiedUsername = model.Kullanici.Username;

                int sonuc = repo_not.Update(result.Nesne);
                if (sonuc > 0)
                {
                    result.Nesne = repo_not.Find(x => x.ID == model.ID);
                }
            }
            return result;
        }
        public BusinessLayerResult<Note> NoteSil(int id)
        {
            
            Note n = repo_not.Find(x => x.ID == id);
            
            if (n != null)
            {
                int sonuc = repo_not.Delete(n);
                if (sonuc == 0)
                {
                    result.hata.Add("Not silinemedi.");
                    return result;
                }
            }
            else
            {
                result.hata.Add("Not bulunamadı");
            }
            return result;
        }


    }
}
