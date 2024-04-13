namespace Common.Domain.Exceptions;
public sealed class NotFoundException : Exception
{
    public NotFoundException(object Id)
        : base($"Resource id {Id} was not found") { }
}
