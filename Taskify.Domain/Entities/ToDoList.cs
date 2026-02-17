using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Common.Classes;

namespace Taskify.Domain.Entities
{
    public class ToDoList : FullyAuditedBaseEntity<int>
    {
        #region Properties
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        #endregion

        #region ForeignKeys
        public int? TeamId { get; set; }
        public Guid? ApplicationUserId { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<TaskItem> Items { get; set; }
        public Team Team { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        #endregion
    }
}
