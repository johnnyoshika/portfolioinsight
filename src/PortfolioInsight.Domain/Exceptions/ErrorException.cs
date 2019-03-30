using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PortfolioInsight.Exceptions
{
    public class ErrorException : Exception
    {
        public ErrorException(params string[] errors)
            : this(HttpStatusCode.BadRequest, errors.AsEnumerable())
        {
        }

        public ErrorException(IEnumerable<string> errors)
            : this(HttpStatusCode.BadRequest, errors)
        {
        }

        public ErrorException(HttpStatusCode statusCode, params string[] errors)
            : this(statusCode, errors.AsEnumerable())
        {
        }

        public ErrorException(HttpStatusCode statusCode, IEnumerable<string> errors)
            : base(string.Join(Environment.NewLine, errors))
        {
            if (statusCode == HttpStatusCode.InternalServerError)
                throw new NotSupportedException("Internal Server Error not supported.");

            StatusCode = statusCode;
            Errors = errors;
        }

        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
