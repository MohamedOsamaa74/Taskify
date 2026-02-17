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
        public bool IsDone { get; set; }
        #endregion

        #region Foreign Keys
        public int ToDoListId { get; set; }
        #endregion

        #region Navigation Property
        public ToDoList ToDoList { get; set; }
        #endregion
    }
}
