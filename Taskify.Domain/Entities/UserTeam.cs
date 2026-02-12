using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Common.Classes;

namespace Taskify.Domain.Entities
{
    public class UserTeam : FullyAuditedBaseEntity<int>
    {
        #region Foreign Keys
        public int UserId { get; set; }
        public int TeamId { get; set; }
        #endregion

        #region Navigation Properties
        public Team Team { get; set; }
        public ApplicationUser User { get; set; }
        #endregion
    }
}
