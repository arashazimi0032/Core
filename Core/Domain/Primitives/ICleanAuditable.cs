namespace Core.Domain.Primitives;

public interface ICleanAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
}
