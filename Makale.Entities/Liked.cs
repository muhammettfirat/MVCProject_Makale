using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.Entities
{
    [Table("Likes")]
    public class Liked
    {
        [Key]
        public int ID { get; set; }
        public virtual Note Note { get; set; }
        public virtual Kullanici Kullanici { get; set; }


    }
}
