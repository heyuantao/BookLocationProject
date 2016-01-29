using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    // 这个接口用于实现其他线程更新UI的功能
    public interface IDispatcherService
    {
        void Dispatch(Action action);
    }
}
