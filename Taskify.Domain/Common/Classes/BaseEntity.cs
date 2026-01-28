using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Common.Interfaces;

namespace Taskify.Domain.Common.Classes
{
    public abstract class BaseEntity<TKEY> : IBaseEntity<TKEY>
    {
        public TKEY Id { get; set; }
    }
}
