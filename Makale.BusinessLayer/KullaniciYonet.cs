using Makale.Common;
using Makale.DataAccsessLayer;
using Makale.Entities;
using Makale.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class KullaniciYonet
    {
        Repository<Kullanici> repo_kul = new Repository<Kullanici>();
        BusinessLayerResult<Kullanici> result = new BusinessLayerResult<Kullanici>();

        public BusinessLayerResult<Kullanici> KullaniciKaydet(RegisterViewModel model)
        {

            result.Nesne= repo_kul.Find(x => x.Username == model.Username || x.Email == model.Email);

            if (result.Nesne != null)
            {
                if (result.Nesne.Username == model.Username)
                {
                    result.hata.Add("Kullanıcı adı kayıtlı.");
                }
                if (result.Nesne.Email == model.Email)
                {
                    result.hata.Add("Email kayıtlı.");
                }
            }
            else
            {
                int sonuc = repo_kul.Insert(new Kullanici()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false

                });
                if (sonuc>0)
                {
                    result.Nesne = repo_kul.Find(x => x.Username == model.Username && x.Email == model.Email);
                    //Aktivasyon maili gönderilecek
                    string siteURL = ConfigHelper.Get<string>("SiteRootUri");
                    string activateURL = $"{siteURL}/Home/UserActivate/{result.Nesne.ActivateGuid}";   //$"":string format 
                    string body = $"Merhaba{result.Nesne.Name}{result.Nesne.Surname} <br> Hesabınızı aktifleştirmek için, <a href='{activateURL}' target='_blank'> tıklayınız </a>";   //_blank :yeni sekmede aç
                    MailHelper.SendMail(body, result.Nesne.Email, "Hesap Aktifleştirme");
                }
            }
            return result;
        }
        
        public BusinessLayerResult<Kullanici> LoginUser(LoginViewModel model)
        {
            result.Nesne = repo_kul.Find(x => x.Username == model.Username && x.Password == model.Password);
            if (result.Nesne!=null)     
            {
                if (!result.Nesne.IsActive)  //false ise
                {
                    result.hata.Add("Kullanıcı aktifleştirilmemiştir.Lütfen e-postanızı kontrol ediniz.");
                }
            }
            else
            {
                result.hata.Add("Kullanıcı adı ya da şifre uyuşmuyor.");
            }
            return result;
        }

        public BusinessLayerResult<Kullanici> ActivateUser(Guid id)
        {
            result.Nesne = repo_kul.Find(x => x.ActivateGuid == id);
            if (result.Nesne!=null)
            {
                if (result.Nesne.IsActive)
                {
                    result.hata.Add("Kullanıcı zaten aktif edilmiştir.");
                }
                result.Nesne.IsActive = true;
                repo_kul.Update(result.Nesne);
            }
            else
            {
                result.hata.Add("Aktifleştirilecek kullanıcı bulunamadı.");
            }
            return result;
        }

        public BusinessLayerResult<Kullanici> KullaniciGetir(int id)
        {
            result.Nesne = repo_kul.Find(x => x.ID == id);
            if (result.Nesne==null)
            {
                result.hata.Add("Kullanıcı bulunamadı.");
            }
            return result;
        }

        public BusinessLayerResult<Kullanici> KullaniciUpdate(Kullanici model)
        {

            Kullanici kullanici = repo_kul.Find(x => x.Username == model.Username || x.Email == model.Email);

            if (kullanici != null && kullanici.ID != model.ID) 
            {
                if (kullanici.Username == model.Username)
                {
                    result.hata.Add("Kullanıcı adı kayıtlı");
                }
                if (kullanici.Email == model.Email)
                {
                    result.hata.Add("Email kayıtlı.");
                }
            }

            result.Nesne = repo_kul.Find(x => x.ID == model.ID);
           
            result.Nesne.Name = model.Name;
            result.Nesne.Surname = model.Surname;
            result.Nesne.Username = model.Username;
            result.Nesne.Email = model.Email;
            result.Nesne.Password = model.Password;

            if (string.IsNullOrEmpty(model.ProfileImageFile)==false)
            {
                result.Nesne.ProfileImageFile = model.ProfileImageFile;
            }
            int sonuc = repo_kul.Update(result.Nesne);
            
            if (sonuc==0)
            {
                result.hata.Add("Profil güncellenemedi.");
            }
            return result;

        }

        public BusinessLayerResult<Kullanici> KullaniciSil(int id)
        {
            Kullanici k = repo_kul.Find(x => x.ID == id);
            if (k!=null)
            {
                int sonuc = repo_kul.Delete(k);
                if (sonuc==0)
                {
                    result.hata.Add("Kullanıcı silinemedi.");
                    return result;
                }
            }
            else
            {
                result.hata.Add("Kullanıcı bulunamadı");
            }
            return result;
        }
    }
}
