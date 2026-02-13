using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.ResultPattern
{
    public class Error
    {
        public string Description { get; set; }

        public Error(string description)
        {
            Description = description;
        }
    }
}
