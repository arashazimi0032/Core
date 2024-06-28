namespace Core.Domain.Enums;

public enum CleanBaseExceptionCode
{
    Exception = 1,
    SystemException = 2,
    ApplicationException = 3,
    ArgumentException = 4,
    ArgumentNullException = 5,
    ArgumentOutOfRangeException = 6,
    InvalidOperationException = 7,
    NotSupportedException = 8,
    NullReferenceException = 9,
    IndexOutOfRangeException = 10,
    DivideByZeroException = 11,
    FormatException = 12,
    IOException = 13,
    TimeoutException = 14,
    OutOfMemoryException = 15,

    CleanBaseSettingException = 1000,
    CleanResultException = 1001,
    CleanBaseRedisCachingException = 1002,
    CleanBaseMemoryCachingException = 1003,
    CleanBaseGrpcClientException = 1004,
    CleanGrpcClientFactoryException = 1005,
    CleanRabbitMqConnectionFactoryException = 1006,
    CleanBaseRabbitMqPublisherException = 1007,
}
