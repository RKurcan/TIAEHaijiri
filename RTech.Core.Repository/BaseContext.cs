using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.DB
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {

        static BaseContext()
        {
            Database.SetInitializer<TContext>(null);
        }

        protected BaseContext()
            : base("name=AttendanceConnection")
        {

        }
    }
}
