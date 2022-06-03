using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale.BusinessLayer;
using Makale.Entities;
using MVCProject_Makale.Filter;

namespace MVCProject_Makale.Controllers
{
    [Exc]
    public class NoteController : Controller
    {

        NoteYonet ny = new NoteYonet();
        LikeYonet ly = new LikeYonet();
        KategoriYonet ky = new KategoriYonet();

        [Auth]
        public ActionResult Index()
        {
            Kullanici user = (Kullanici)Session["login"];
            return View(ny.ListQueryable().Include("Kategori").Include("Kullanici").Where(x => x.Kullanici.ID == user.ID).OrderByDescending(x => x.ModifiedDate).ToList());
            //return View(ny.NotListesi().Where(x=>x.Kullanici==user));
        }

        [Auth]
        public ActionResult LikedNotes()
        {
            Kullanici user = (Kullanici)Session["login"];
            var notes = ly.ListQueryable().Include("Kullanici").Include("Note").Where(x => x.Kullanici.ID == user.ID).Select(x => x.Note).Include("Kategori").Include("Kullanici").OrderByDescending(x => x.ModifiedDate);
            return View("Index", notes.ToList());        
        }

        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            Kullanici user = (Kullanici)Session["login"];
            List<int> listid = new List<int>();

            if (user!=null)
            {   
              if (ids !=null)
              {
                   listid = ly.List(x => x.Kullanici.ID == user.ID && ids.Contains(x.Note.ID)).Select(x => x.Note.ID).ToList();
                }
                else
                {
                    listid = ly.ListQueryable().Where(x => x.Kullanici.ID == user.ID).Select(x => x.Note.ID).ToList();
                }

                return Json(new { sonuc = listid });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }

        }

        [HttpPost]
        public ActionResult SetLiked(int notid,bool liked)
        {
            Kullanici user =(Kullanici)Session["login"];
            int sonuc = 0;

            if (user == null)
               return Json(new { hata = true, mesaj = "Beğeni için giriş yapmalısınız.", sonuc});

            Liked like = ly.LikeBul(notid,user.ID);
            Note not = ny.NotBul(notid);

            if (like != null && liked == false)
            {
                sonuc=ly.LikeSil(like);
            }
            else if (like == null && liked == true)
            {
                sonuc=ly.LikeInsert(new Liked() { Kullanici = user,Note=not });
            
            }

            if(sonuc>0)
            {
                if (liked)
                    not.LikeCount++;
                else
                    not.LikeCount--;

                ny.NoteUpdate(not);
                return Json(new { hata = false, sonuc = not.LikeCount,mesaj=string.Empty });
            }

            return Json(new { hata = true, sonuc = not.LikeCount, mesaj = "Beğeni gerçekleşmedi." });

        }
        
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = ny.NotBul(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [Auth]
        public ActionResult Create()
        {
            ViewBag.KategoriId = new SelectList(ky.KategoriListesi(), "ID", "Title");
            return View();
        }

        
        [HttpPost]
        [Auth]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatDate");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                note.Kullanici = (Kullanici)Session["login"];
                ny.NoteKaydet(note);
                return RedirectToAction("Index");
            }
            ViewBag.KategoriId = new SelectList(ky.KategoriListesi(), "ID", "Title",note.KategoriId);
            return View(note);
        }

        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = ny.NotBul(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriId = new SelectList(ky.KategoriListesi(), "ID", "Title", note.KategoriId);

            return View(note);
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Note note)
        {
            ModelState.Remove("CreatDate");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                note.Kullanici = (Kullanici)Session["login"];
                ny.NoteUpdate(note);
                return RedirectToAction("Index");
            }
            ViewBag.KategoriId = new SelectList(ky.KategoriListesi(), "ID", "Title", note.KategoriId);

            return View(note);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = ny.NotBul(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusinessLayerResult<Note> sonuc = ny.NoteSil(id);
           
            
            return RedirectToAction("Index");
        }

        public ActionResult NotDetay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = ny.NotBul(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialNotDetay", note);

        }


    }
}
