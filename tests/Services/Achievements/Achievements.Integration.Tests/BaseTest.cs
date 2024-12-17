using Achievements.Infrastructure.Persistence;
using Common.Tests;
using Common.Tests.Utils;
using MassTransit.Testing;

namespace Achievements.Integration.Tests;

[Trait("Category", "Integration")]
[Collection(nameof(IntegrationTestCollection<IntegrationTestWebAppFactory>))]
public abstract class BaseTest : IAsyncLifetime
{
    private protected readonly ServiceDbContext Db;
    protected HttpClient ServiceClient;
    protected Func<Task> ResetDb;
    protected ITestHarness Harness;

    public BaseTest(IntegrationTestWebAppFactory factory)
    {
        Db = factory.Db;

        ResetDb = factory.ResetDatabase;

        Harness = factory.Services.GetTestHarness();
        ServiceClient = factory.CreateClient();
    }

    public async Task Insert<T>(T entity) where T : class
    {
        Db.Add(entity);
        await Db.SaveChangesAsync();
        Db.ChangeTracker.Clear();
    }

    public void AddToken(int userId = 1)
    {
        var token = TestTokenGenerator.Create(userId);
        ServiceClient.DefaultRequestHeaders.Authorization = new("Bearer", token);
    }

    public async Task InitializeAsync()
    {
        await ResetDb();
    }

    public async Task DisposeAsync()
    {
        await ResetDb();
    }
}