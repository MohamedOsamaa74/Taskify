using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Domain.Common.Interfaces
{
    public interface IDeletable
    {
        bool IsDeletable { get; set; }
        DateTime? DeletedAt { get; set; }
        int? DeletedBy { get; set; }
    }
}
