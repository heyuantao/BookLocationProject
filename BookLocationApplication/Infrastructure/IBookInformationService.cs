using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IBookInformationService
    {
        String ServerIp { get; set; }
        String ServerPort { get; set; }
        String ServerUsername { get; set; }
        String ServerPassword { get; set; }
        List<String> getBookAccessCodeListByRfidList(List<String> rfidList);
        List<String> getBookNameListByRfidList(List<String> rfidList);
    }
}
