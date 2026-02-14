using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.DTOs.AccountDTOs
{
    public class RegisterUserDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
