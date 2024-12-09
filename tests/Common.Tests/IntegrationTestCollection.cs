namespace Common.Tests;

public class IntegrationTestCollection<TClass>
    : ICollectionFixture<TClass>
    where TClass : class
{
}
