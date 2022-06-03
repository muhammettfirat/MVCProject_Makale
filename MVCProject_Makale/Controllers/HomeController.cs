using Makale.BusinessLayer;
using Makale.Entities;
using Makale.Entities.ViewModel;
using MVCProject_Makale.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCProject_Makale.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        // GET: Home


        NoteYonet ny = new NoteYonet();
        KategoriYonet ky = new KategoriYonet();
        KullaniciYonet kuly = new KullaniciYonet();
        public ActionResult Index()
        {
            //Makale.BusinessLayer.Test test = new Makale.BusinessLayer.Test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.CommentTest();

            //return View(ny.NotListesi().OrderByDescending(x => x.ModifiedDate).ToList());

            return View(ny.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedDate).ToList());
        }

       
        public ActionResult Kategori(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            //Kategori kat = ky.KategoriBul(id.Value);
            //return View("Index", kat.Notes);

            List<Note> notes = ny.ListQueryable().Where(
           x => x.IsDraft == false && x.KategoriId == id).OrderByDescending(
           x => x.ModifiedDate).ToList();

            return View("Index", notes);
        }
        public ActionResult Popular()
        {
            NoteYonet ny = new NoteYonet();
            return View("Index", ny.NotListesi().OrderByDescending(x => x.LikeCount).Take(6).ToList());           
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                KullaniciYonet ky = new KullaniciYonet();   //burdan şunu anlıyorum böyle bir class var ve static değil
                BusinessLayerResult<Kullanici> sonuc = ky.LoginUser(model);
                if (sonuc.hata.Count>0)
                {
                    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                Session["login"] = sonuc.Nesne;
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost] 
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)   //model geçerliyse içeri gir ama tek bir hata varsa false olur.
            {
                //kullanıcı username ve e posta kontrolu yapılacak
                //kayıt işlemi gerçekleştirilecek
                //aktivasyon maili gönderilecek

                KullaniciYonet ky = new KullaniciYonet();
                BusinessLayerResult<Kullanici> sonuc = ky.KullaniciKaydet(model);

                if (sonuc.hata.Count>0)
                {
                    sonuc.hata.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }

                return RedirectToAction("RegisterOK");
            }
            return View();
        }
        public ActionResult RegisterOK()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult UserActivate(Guid id)
        {
            KullaniciYonet ky = new KullaniciYonet();
            BusinessLayerResult<Kullanici> sonuc = ky.ActivateUser(id);
            if (sonuc.hata.Count>0)
            {
                TempData["error"] = sonuc.hata;
                return RedirectToAction("UserActivateHata");
            }
            return RedirectToAction("UserActivateOK");
        }
        public ActionResult UserActivateHata()
        {
            List<string> hatalar = new List<string>();
            if (TempData["error"]!=null)
            {
                hatalar = (List<string>)TempData["error"];
            }
            return View(hatalar);
        }
        public ActionResult UserActivateOK()
        {            
            return View();
        }

        [Auth]
        public ActionResult Profile()
        {
            Kullanici user = (Kullanici)Session["login"];
            BusinessLayerResult<Kullanici> sonuc = kuly.KullaniciGetir(user.ID);
            if (sonuc.hata.Count>0)
            {
                //hata varsa kullanıcıyı başka sayfaya yönlendir.
            }
            return View(sonuc.Nesne);
        }
        
        [Auth]
        public ActionResult ProfileDuzenle()
        {
            Kullanici user = (Kullanici)Session["login"];
            BusinessLayerResult<Kullanici> sonuc = kuly.KullaniciGetir(user.ID);
            if (sonuc.hata.Count > 0)
            {
                //hata varsa kullanıcıyı başka sayfaya yönlendir.
            }
            return View(sonuc.Nesne);
            
        }

        [Auth]
        [HttpPost]
        public ActionResult ProfileDuzenle(Kullanici user,HttpPostedFileBase profileimage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (profileimage!=null && (profileimage.ContentType=="image/png" || profileimage.ContentType=="image/jpg" || profileimage.ContentType=="image/jpeg"))
                {
                    string dosyaadi = $"user_{user.ID}.{profileimage.ContentType.Split('/')[1]}";  //kullanıcının attığı isim ne olursa olsun biz "user_1.jpg" ya da "user_5.png" gibi kayıt etmiş olucaz
                    profileimage.SaveAs(Server.MapPath($"~/Image/{dosyaadi}"));
                    user.ProfileImageFile = dosyaadi;
                }
                BusinessLayerResult<Kullanici> sonuc = kuly.KullaniciUpdate(user);
                if (sonuc.hata.Count>0)
                {
                    //işlemler return
                }
                Session["login"] = sonuc.Nesne;
                return RedirectToAction("Profile");
            }
            return View(user);
        }

        [Auth]
        public ActionResult ProfileSil()
        {
            Kullanici kullanici = (Kullanici)Session["login"];
            BusinessLayerResult<Kullanici> sonuc = kuly.KullaniciSil(kullanici.ID);
            if (sonuc.hata.Count>0)
            {
                //hata sayfasına yönlendirme
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
        

        public ActionResult HataliErisim()
        {
            return View();
        }

        public ActionResult Errors()
        {
            return View();
        }

        public ActionResult Viewdeneme()
        {
            return View();
        }
       
       
       
    }
}