using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.Entities
{

    public class EntitiesBase
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Oluşturma Tarihi"),Required]
        public DateTime CreateDate { get; set; }
        [DisplayName("Değiştirme Tarihi"),Required]
        public DateTime ModifiedDate { get; set; }
        [DisplayName("Değiştirilen Kullanıcı Adı"), Required, StringLength(30)]
        public string ModifiedUsername { get; set; }

    }
}
