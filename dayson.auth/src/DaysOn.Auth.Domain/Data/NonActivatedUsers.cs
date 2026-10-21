using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaysOn.Common.DataAccess;

namespace DaysOn.Auth.Domain.Data
{
    public class NonActivatedUsers : TableEntity
    {
        public List<Guid> Users { get;set; }
    }
}
