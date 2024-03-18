using Core.Application.ServiceLifeTimes;
using System.Net;

namespace Core.Domain.Exceptions;

internal interface ICleanBaseException : ICleanBaseIgnore
{
    HttpStatusCode HttpStatusResponseType { get; }

    ExceptionMessage GetMessage();
}
