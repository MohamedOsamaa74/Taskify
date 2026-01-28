using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Common.Interfaces;

namespace Taskify.Domain.Common.Classes
{
    public class AuditableBaseEntity<TKEY> : BaseEntity<TKEY>, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
