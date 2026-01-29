using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Common.Classes;

namespace Taskify.Domain.Entities
{
    public class UserGroup : FullyAuditedBaseEntity<int>
    {
        #region Foreign Keys
        public int UserId { get; set; }
        public int GroupId { get; set; }
        #endregion

        #region Navigation Properties
        public Group Group { get; set; }
        public ApplicationUser User { get; set; }
        #endregion
    }
}
