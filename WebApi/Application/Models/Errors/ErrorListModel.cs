using System.Collections.Generic;

namespace WebApi.Application.Models.Errors
{
    public class ErrorListModel
    {
        public IList<ErrorMessageModel> Errors { get; set; }
    }
}