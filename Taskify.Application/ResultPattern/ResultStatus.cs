using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.ResultPattern
{
    public enum ResultStatus
    {
        ok = 200,
        Created = 201,
        BadRequest = 400,
        UnAuthorized = 401,
        NotFound = 404,
        InternalServerError = 500
    }
}
