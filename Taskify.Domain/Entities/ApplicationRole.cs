using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Common.Interfaces;

namespace Taskify.Domain.Entities
{
    public class ApplicationRole : IdentityRole<Guid>, IAuditable, IDeletable
    {
        public string NameAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeletable { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
