using System.Collections.Generic;

namespace WebApi.Common.Models.Errors
{
    public class ErrorListModel
    {
        public IList<ErrorMessageModel> Errors { get; set; }
    }
}