using Core.Application.ServiceLifeTimes;
using Microsoft.AspNetCore.Http;

namespace Core.Presentation.Middleware;

public interface ICleanBaseMiddleware : IMiddleware, ICleanBaseTransient
{
}
