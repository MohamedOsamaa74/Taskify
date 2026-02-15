using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.DTOs.UserTeamDTOs
{
    public class UserTeamDTO
    {
        public Guid UserId { get; set; }
        public int TeamId { get; set; }
    }
}
