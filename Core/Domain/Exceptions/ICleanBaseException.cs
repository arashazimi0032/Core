using Core.Application.ServiceLifeTimes;
using System.Net;

namespace Core.Domain.Exceptions;

public interface ICleanBaseException : ICleanBaseIgnore
{
    HttpStatusCode HttpStatusResponseType { get; }

    ExceptionMessage GetMessage();
}
