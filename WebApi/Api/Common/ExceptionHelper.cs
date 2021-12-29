using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebApi.Application.Exceptions;
using WebApi.Application.Models.Errors;

namespace Api.Common
{
    public static class ExceptionHelper
    {
        public static IApplicationBuilder UseConfigureExeptions(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error is ValidationException vExc)
                            await context.Response.WriteAsync($"{vExc.Message}");
                        else
                        {
                            var message = JsonConvert.SerializeObject(new ErrorListModel
                            {
                                Errors = new List<ErrorMessageModel> {new() {Code = 2, Message = contextFeature.Error.Message}}
                            });
                            await context.Response.WriteAsync($"{message}");
                        }
                    }
                });
            });

            return app;
        }
    }
}