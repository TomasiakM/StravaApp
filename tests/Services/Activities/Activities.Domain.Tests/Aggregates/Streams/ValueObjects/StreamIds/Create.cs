using Activities.Domain.Aggregates.Streams.ValueObjects;

namespace Activities.Domain.Tests.Aggregates.Streams.ValueObjects.StreamIds;
public class Create
{
    [Fact]
    public void ShouldCreateActivityId()
    {
        var streamId = StreamId.Create();

        Assert.NotEqual(Guid.Empty, streamId.Value);
    }

    [Fact]
    public void ShouldCreateActivityId2()
    {
        var guid = Guid.NewGuid();
        var streamId = StreamId.Create(guid);

        Assert.Equal(guid, streamId.Value);
    }

    [Fact]
    public void ShouldCreateUniqueStreamIds()
    {
        var streamId = StreamId.Create();
        var streamId2 = StreamId.Create();

        Assert.False(streamId == streamId2);
    }
}
