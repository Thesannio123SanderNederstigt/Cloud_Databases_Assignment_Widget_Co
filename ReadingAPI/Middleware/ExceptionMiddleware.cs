using System.Linq;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Model.Response;
using Service.Exceptions;
using System.Collections.Generic;

namespace API.Middleware;

public class ExceptionMiddleware : IFunctionsWorkerMiddleware
{
    private readonly Dictionary<Type, HttpStatusCode> _statusCodes = new();

    public ExceptionMiddleware()
    {
        AddHandler<NotImplementedException>(HttpStatusCode.NotImplemented);
        AddHandler<NotFoundException>(HttpStatusCode.NotFound);
    }

    internal void AddHandler<TException>(HttpStatusCode statusCode) where TException : Exception
    {
        _statusCodes.Add(typeof(TException), statusCode);
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (await context.GetHttpRequestDataAsync() is HttpRequestData req)
            {
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

                if (ex is AggregateException ae)
                {
                    ex = ae.InnerException!;
                }

                if (_statusCodes.TryGetValue(ex.GetType(), out HttpStatusCode code))
                {
                    statusCode = code;
                }

                HttpResponseData res = req.CreateResponse(statusCode);

                await res.WriteAsJsonAsync(new ErrorResponse(ex), statusCode);

                InvocationResult invocation = context.GetInvocationResult();
                OutputBindingData<HttpResponseData> binding = context.GetOutputBindings<HttpResponseData>().FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");

                if (binding is not null)
                {
                    binding.Value = res;
                }
                else
                {
                    invocation.Value = res;
                }
            }
        }
    }
}
