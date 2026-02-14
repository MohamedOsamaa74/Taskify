using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.DTOs.AccountDTOs
{
    public class UserRoleDTO
    {
        public required string UserName { get; set; }
        public required string RoleName { get; set; }
    }
}
