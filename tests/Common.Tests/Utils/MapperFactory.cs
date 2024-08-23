using Mapster;
using MapsterMapper;
using System.Reflection;

namespace Common.Tests.Utils;
public static class MapperFactory
{
    public static IMapper Create(Assembly assembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);
        return new Mapper(config);
    }
}
