using FarmaciaAPI.Middlewares.Exceptions;
using System.Net;
using System.Text.Json;

namespace FarmaciaAPI.Middlewares;

public class MappingErrorMiddleware
{
	private readonly RequestDelegate _request;
  
    public MappingErrorMiddleware(RequestDelegate request)
	{
		_request = request;
	}

	public async Task InvokeAsync(HttpContext context) 
	{
		try
		{
			await _request(context);
		}
		catch (Exception ex)
		{
			switch (ex.GetType().ToString())
			{
				case "NotFoundException":
                    await HandlerExceptionAsync(context, ex);
                    break;
                case "DBConcurrencyException":
                    await HandlerExceptionAsync(context, ex);
                    break;
                default:
                    await InternalServerError(context, ex);
                    break;
            }
		}
	}

    private static Task HandlerExceptionAsync(HttpContext context, Exception ex)
    {
        HttpStatusCode statusCode;
        string? stackTrace = String.Empty;
        string mensagem;

        mensagem = ex.Message;
        statusCode = HttpStatusCode.BadRequest;
        stackTrace = ex.StackTrace;
        
		var result = JsonSerializer.Serialize(new { statusCode, mensagem, stackTrace });

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)statusCode;
		return context.Response.WriteAsync(result);
    }

	private static Task InternalServerError(HttpContext context, Exception ex) 
	{
        HttpStatusCode statusCode;
        string? stackTrace = String.Empty;
        string mensagem;

        mensagem = ex.Message;
        statusCode = HttpStatusCode.InternalServerError;
        stackTrace = ex.StackTrace;

        var result = JsonSerializer.Serialize(new { statusCode, mensagem, stackTrace });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }
}

//https://www.educative.io/answers/how-to-catch-multiple-exceptions-at-once-in-c-sharp
//https://www.treinaweb.com.br/blog/tratando-erros-em-uma-api-asp-net-core-com-middleware
//https://www.youtube.com/watch?v=U4am8b9nOao
//https://www.treinaweb.com.br/blog/criando-um-middleware-customizado-para-asp-net-core