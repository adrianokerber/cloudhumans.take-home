using CloudHumans.ClaudIA.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudHumans.ClaudIA.Infrastructure;

public sealed class HttpResponseFactory : IService<HttpResponseFactory>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpResponseFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IResult CreateSuccessWith200(object data) =>
        Results.Ok(new
        {
            Status = StatusCodes.Status200OK,
            Message = "Ok",
            Data = data
        });

    public IResult CreateErrorWith400(string message, string details) =>
        Results.BadRequest(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = message,
            Type = "Failure",
            Detail = details,
            Instance = _httpContextAccessor.HttpContext!.Request.Path
        });
    
    public IResult CreateErrorWith500(string details) =>
        Results.Problem(new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal server error",
            Type = "Critical",
            Detail = details,
            Instance = _httpContextAccessor.HttpContext!.Request.Path
        });
}