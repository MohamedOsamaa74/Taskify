using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.DTOs.ToDoListDTOs
{
    public class ToDoListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? TeamId { get; set; }
        public string? TeamName { get; set; }
        public Guid? ApplicationUserId { get; set; }
        public string? UserName { get; set; }
    }
}
