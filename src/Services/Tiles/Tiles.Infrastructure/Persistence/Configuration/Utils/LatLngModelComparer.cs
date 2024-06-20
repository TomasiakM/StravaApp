using Common.Domain.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Tiles.Infrastructure.Persistence.Configuration.Utils;
internal sealed class LatLngModelComparer
    : ValueComparer<List<LatLng>>
{
    public LatLngModelComparer()
    : base((c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList())
    { }
}
