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
    [Table("Kategoriler")]
    public class Kategori:EntitiesBase
    {
        [DisplayName("Kategori"),Required,StringLength(50)]
        public string Title { get; set; }
        [DisplayName("Açıklama"), StringLength(150)]
        public string Description { get; set; }
        public virtual List<Note> Notes { get; set; } //Bir kategoride birden fazla makale vardır.
        public Kategori()
        {
            Notes = new List<Note>();
        }
    }
}
