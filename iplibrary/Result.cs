using System;
namespace iplibrary
{
    public class Result<TResultType>
    {
        public bool Success { get; private set; }

        public TResultType Body { get; private set; }

        public Error Error { get; private set; }

        public Result(bool s,TResultType b,Error e)
        {
            Success = s;
            Body = b;
            Error = e;
        }
    }

    public class Error
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public Error(string c, string m)
        {
            Code = c;
            Message = m;
        }
    }
}
