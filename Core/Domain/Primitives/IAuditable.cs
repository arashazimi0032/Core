﻿namespace Core.Domain.Primitives;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
}