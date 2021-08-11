using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.Common
{
    public interface

         IBaseService<t>
    {
        ServiceResult<IQueryable<t>> List();
        ServiceResult<t> Add(t model);
        ServiceResult<t> Update(t model);
        ServiceResult<int> Remove(t model);
    }
}
