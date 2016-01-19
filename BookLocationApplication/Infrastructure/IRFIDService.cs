using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IRFIDService
    {
        String HardwareInterface { get; set; }
        String HardwareInterfaceConnectionSpeed { get; set; }
        void start();
        void stop();
    }
}
