using System;
using System.Collections.Generic;
using System.Text;

namespace Taskify.Application.ResultPattern
{
    public class Result<T>
    {
        public string Message { get; init; }
        public bool IsSuccess { get; init; }
        public int StatusCode { get; init; }
        public T? Data;
        public bool IsError => !IsSuccess;
        private Result(bool isSuccess, string message, ResultStatus status)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = (int)status;
        }
        private Result(bool isSuccess, string message, T data, ResultStatus status)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            StatusCode = (int)status;
        }
        public static Result<T> Created(T Data) => new(true, "Created Successfully", Data, ResultStatus.Created);
        public static Result<T> Updated(T Data) => new(true, "Updated Successfully", Data, ResultStatus.ok);
        public static Result<T> Deleted() => new(true, "Deleted Successfully", ResultStatus.ok);
        public static Result<T> Success(T Data) => new(true, "Success", Data, ResultStatus.ok);
        public static Result<T> Success() => new(true, "Success", ResultStatus.ok);
        public static Result<T> Failure(string message) => new(false, message, ResultStatus.BadRequest);
        public static Result<T> NotFound(string message) => new(false, message, ResultStatus.NotFound);
        public static Result<T> UnAuthorized() => new(false, "UnAuthorized", ResultStatus.UnAuthorized);
    }
}
