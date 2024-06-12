using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ZeroGravity.Constants;
using ZeroGravity.Helpers;

namespace ZeroGravity.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                ErrorCode errorCode = ErrorCode.UnknownError;
                switch(error)
                {
                    case AccountNotFoundException e:
                        errorCode = ErrorCode.AccountNotFound;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case AccountNotVerifiedException e:
                        errorCode = ErrorCode.AcountNotVerified;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case EmailOrPasswordIncorrectException e:
                        errorCode = ErrorCode.EmailOrPasswordIncorrect;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case AppException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new {
                    message = error?.Message,
                    error_code = errorCode
                });
                await response.WriteAsync(result);
            }
        }
    }
}