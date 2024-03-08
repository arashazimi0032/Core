using Microsoft.AspNetCore.Routing;
using Core.Application.ServiceLifeTimes;

namespace Core.Presentation.Endpoint;

public interface ICleanBaseEndpoint : ICleanBaseIgnore
{
    public void AddRoutes(IEndpointRouteBuilder app);
}
