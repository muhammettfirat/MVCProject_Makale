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
    [Table("Notes")]
    public class Note:EntitiesBase
    {
        [DisplayName("Not Başlığı"),Required,StringLength(60)]
        public string Title { get; set; }

        [DisplayName("Metin"), Required, StringLength(2000)]
        public string Text { get; set; }
        [DisplayName("Beğeni Sayısı")]
        public int LikeCount { get; set; }
        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }
        [DisplayName("Kategori")]
        public int KategoriId { get; set; }
        public virtual Kategori Kategori { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual List<Liked> Likes { get; set; }

        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }
    }
}
