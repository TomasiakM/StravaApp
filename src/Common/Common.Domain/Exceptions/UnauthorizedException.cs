namespace Common.Domain.Exceptions;
public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException()
        : base("Authentication failed") { }
}
