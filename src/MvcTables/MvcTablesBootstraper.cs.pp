using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcTables.Configuration;

namespace $rootnamespace$
{
    public class MvcTablesBootstraper
    {
        public static void Bootstrap()
        {
            ConfigureMvcTables.InTheSameAssembly.As<MvcTablesBootstraper>();
        }
    }
}
