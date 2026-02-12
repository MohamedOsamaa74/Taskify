using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Domain.Common.Interfaces
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        Guid? UpdatedBy { get; set; }
    }
}
