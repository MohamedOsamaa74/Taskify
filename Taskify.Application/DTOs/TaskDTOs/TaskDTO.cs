using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.DTOs.TaskDTOs
{
    public class TaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        public int ToDoListId { get; set; }
        public string ToDoListName { get; set; }
    }
}
