namespace Common.Domain.Models;
public record Activity(
    Guid Id,
    double Distance,
    DateTime StartDateLocal);
