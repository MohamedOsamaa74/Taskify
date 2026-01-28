using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Domain.Common.Interfaces
{
    public interface IBaseEntity<TKEY>
    {
        public TKEY Id { get; set; }
    }
}
