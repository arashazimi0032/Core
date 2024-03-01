using Core.Application.ServiceLifeTimes;

namespace Core.Domain.Primitives;

public interface ICleanAuditable : ICleanBaseIgnore
{
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
}
