using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.DTOs.AccountDTOs
{
    public class LoginUserDTO
    {
        public string LoginIdentifier { get; set; }
        public string Password { get; set; }
    }
}
