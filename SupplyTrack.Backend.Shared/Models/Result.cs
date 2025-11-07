using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Shared.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsFailure=> !IsSuccess;
        protected Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        protected Result(bool isSuccess, string errorMessage) : this(isSuccess)
        {
            if (isSuccess && !string.IsNullOrEmpty(errorMessage))
                throw new InvalidOperationException("A successful result cannot have an error message.");
            if (!isSuccess && string.IsNullOrEmpty(errorMessage))
                throw new InvalidOperationException("A failure result must have an error message.");
            ErrorMessage = errorMessage;
        }
        public static Result Success() => new(true);
        public static Result Failure(string errorMessage) => new(false,errorMessage);
    }
}
