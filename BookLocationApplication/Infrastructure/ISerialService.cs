using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISerialService
    {
        List<String> SerialList();
        String Serial { get; set; }
        String Speed {get;set;}
    }
}
