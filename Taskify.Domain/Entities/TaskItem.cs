using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Common.Classes;

namespace Taskify.Domain.Entities
{
    public class TaskItem : FullyAuditedBaseEntity<int>
    {
        #region Properties
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        #endregion

        #region Foreign Keys
        public int AssignedToUserId { get; set; }
        public int ListId { get; set; }
        #endregion

        #region Navigation Property
        public ApplicationUser User { get; set; }
        public ToDoList ToDoList { get; set; }
        #endregion
    }
}
