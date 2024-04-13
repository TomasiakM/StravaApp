﻿namespace Common.Domain.Exceptions;
public sealed class ForbiddenException : Exception
{
    public ForbiddenException()
        : base("Access denided") { }
}
