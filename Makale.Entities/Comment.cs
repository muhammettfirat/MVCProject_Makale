using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makale.Entities
{
    [Table("Comments")]
    public class Comment:EntitiesBase
    {
        [Required,StringLength(300)]
        public string Text { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual Note Note { get; set; }

    }
}
