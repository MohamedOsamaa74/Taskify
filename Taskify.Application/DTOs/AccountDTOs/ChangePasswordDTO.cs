using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.DTOs.AccountDTOs
{
    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
