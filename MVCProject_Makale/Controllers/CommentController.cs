using Makale.BusinessLayer;
using Makale.Entities;
using MVCProject_Makale.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCProject_Makale.Controllers
{
    [Exc]
    public class CommentController : Controller
    {
        
        // GET: Comment
        public ActionResult YorumGoster(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NoteYonet ny = new NoteYonet();
            Note not = ny.NotBul(id.Value);
            if (not==null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialComments",not.Comments);
        }
        YorumYonet yy = new YorumYonet();
        NoteYonet ny = new NoteYonet();

        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id,string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = yy.YorumBul(id.Value);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            comment.Text = text.Trim();
            if (yy.YorumUpdate(comment)>0)
            {
                return Json(new { sonuc = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { sonuc = false }, JsonRequestBehavior.AllowGet);
            
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = yy.YorumBul(id.Value);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            
            if (yy.YorumSil(comment) > 0)
            {
                return Json(new { sonuc = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { sonuc = false }, JsonRequestBehavior.AllowGet);

        }

        [Auth]
        public ActionResult Create(Comment comment,int? notid)
        {
            ModelState.Remove("CreateDate");
            ModelState.Remove("ModifiedDate");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (notid==null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Note not = ny.NotBul(notid.Value);  //null gelebilir dediğimiz için value yazıyoruz
                if (not==null)
                {
                    return new HttpNotFoundResult();
                }
                comment.Note = not;
                comment.Kullanici = (Kullanici)Session["login"];
                if (yy.YorumKaydet(comment) > 0)
                {
                    return Json(new { sonuc = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { sonuc = false }, JsonRequestBehavior.AllowGet);

        }
    }
}