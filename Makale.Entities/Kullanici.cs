using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Makale.Entities
{
    [Table("Kullanicilar")]
    public class Kullanici:EntitiesBase
    {
        [DisplayName("Isim"),StringLength(25,ErrorMessage ="{0} alanı max.{1} karakter olmalıdır.")]
        public string Name { get; set; }
        [DisplayName("Soyisim"),StringLength(25, ErrorMessage = "{0} alanı max.{1} karakter olmalıdır.")]
        public string Surname { get; set; }
        [DisplayName("Kullanıcı Adı"),Required(ErrorMessage = "{0} alanı gereklidir."),StringLength(25, ErrorMessage = "{0} alanı max.{1} karakter olmalıdır.")]
        public string Username { get; set; }
        [DisplayName("Email"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(70, ErrorMessage = "{0} alanı max.{1} karakter olmalıdır.")]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(25, ErrorMessage = "{0} alanı max.{1} karakter olmalıdır.")]
        public string Password { get; set; }
        [StringLength(30)]
        public string ProfileImageFile { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public Guid ActivateGuid { get; set; }
        public virtual List<Note> Notes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public  List<Liked> Likes { get; set; }
        public Kullanici()
        {
            Notes = new List<Note>();
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

    }
}
