using Microsoft.Practices.Unity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    public class UIModule:IModule
    {
        IUnityContainer container;
        public UIModule(IUnityContainer container)
        {
            this.container = container;
        }
        public void Initialize()
        {
        }
    }
}
