using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Taskify.Domain.Common.Classes;
using Taskify.Domain.Common.Interfaces;

namespace Taskify.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>, IAuditable, IDeletable
    {
        #region Properties
        [Required]
        public string FullNameEn {  get; set; }
        [Required]
        public string FullNameAr {  get; set; }
        [Required]
        public string MobileNumber {  get; set; }
        #endregion

        #region Audit
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeletable { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<UserTeam> UserTeams { get; set; } = [];
        public ICollection<ToDoList> ToDoLists { get; set; } = [];
        #endregion
    }
}
