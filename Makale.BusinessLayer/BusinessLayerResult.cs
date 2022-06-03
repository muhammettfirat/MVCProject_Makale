using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer
{
    public class BusinessLayerResult<T> where T:class
    {
        public List<string> hata { get; set; }
        public T Nesne { get; set; }
        public BusinessLayerResult()
        {
            hata = new List<string>();
        }

    }
}
