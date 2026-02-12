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
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public bool IsDeletable { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<Team> Teams { get; set; } = [];
        public ICollection<UserTeam> UserTeams { get; set; } = [];
        public ICollection<ToDoList> ToDoLists { get; set; } = [];
        #endregion
    }
}
