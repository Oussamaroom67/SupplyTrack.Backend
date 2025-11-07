using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyTrack.Backend.Shared.Models
{
    public class Result<T>:Result 
    {
        public T? Data { get; set; }
        protected Result(bool isSuccess, T? data) : base(isSuccess)
        {
            Data = data;
        }
        protected Result(bool isSuccess, string errorMessage) : base(isSuccess, errorMessage)
        {
        }
        public static Result<T> Success(T data) => new(true, data);
        public static new Result<T> Failure(string errorMessage) => new(false, errorMessage);
    }
}
