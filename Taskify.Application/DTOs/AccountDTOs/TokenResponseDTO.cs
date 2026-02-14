using System;
using System.Collections.Generic;
using System.Text;
using Taskify.Domain.Entities;

namespace Taskify.Application.DTOs.AccountDTOs
{
    public class TokenResponseDTO
    {
        public string Token { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
