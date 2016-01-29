using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace UI.Services
{
    public class UIDispatcherService : IDispatcherService
    {
        Dispatcher dispatcher;
        public UIDispatcherService(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }
        public void Dispatch(Action action)
        {
            this.dispatcher.BeginInvoke(action);
        }
    }
}
