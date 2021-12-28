using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Newtonsoft.Json;
using WebApi.Common.Models.Errors;

namespace WebApi.Common.Exceptions
{
    public class ValidationException : Exception
    {
        private static int CodeException => 0x1;

        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base(JsonConvert.SerializeObject(new ErrorListModel
            {
                Errors = failures.Select(n => new ErrorMessageModel
                    {
                        Code = CodeException,
                        Message = $"{n.PropertyName}: {n.ErrorMessage}"
                    })
                    .ToList()
            }))
        {
        }
    }
}