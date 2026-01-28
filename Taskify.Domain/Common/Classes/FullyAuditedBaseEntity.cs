using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Common.Interfaces;

namespace Taskify.Domain.Common.Classes
{
    public class FullyAuditedBaseEntity<TKEY> : AuditableBaseEntity<TKEY>, IDeletable
    {
        public bool IsDeletable { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}