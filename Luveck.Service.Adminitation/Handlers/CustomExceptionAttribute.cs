using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;
using Luveck.Service.Administration.Utils.Resource;
using Luveck.Service.Administration.Utils.Exceptions;
using Luveck.Service.Administration.Models;

namespace Luveck.Service.Administration.Handlers
{
    [ExcludeFromCodeCoverage]
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {
        public CustomExceptionAttribute()
        {
        }

        public override void OnException(ExceptionContext context)
        {
            HttpResponseException oResponseExeption = new HttpResponseException();
            ResponseModel<string> oResponse = new ResponseModel<string>()
            {
                IsSuccess = false,
                // Result = JsonConvert.SerializeObject(context.Exception)
            };

            if (context.Exception is BusinessException)
            {
                oResponseExeption.Status = StatusCodes.Status400BadRequest;
                oResponse.Messages = context.Exception.Message;
                context.ExceptionHandled = true;
            }
            else
            {
                if (context.Exception != null)
                {
                    oResponseExeption.Status = StatusCodes.Status500InternalServerError;
                    oResponse.Messages = GeneralMessage.Error500;
                }
                context.ExceptionHandled = true;
            }

            context.Result = new ObjectResult(oResponseExeption.Value)
            {
                StatusCode = oResponseExeption.Status,
                Value = oResponse
            };

            if (oResponseExeption.Status == StatusCodes.Status500InternalServerError)
                context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = GeneralMessage.Error500;
        }
    }
}
