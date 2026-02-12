using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Taskify.Domain.Common.Classes;

namespace Taskify.Domain.Entities
{
    public class Team : FullyAuditedBaseEntity<int>
    {
        #region Properties
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(200)]
        public string Description { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<UserTeam> UserTeams { get; set; } = [];
        public ICollection<ToDoList> ToDoLists { get; set; } = [];
        #endregion
    }
}
