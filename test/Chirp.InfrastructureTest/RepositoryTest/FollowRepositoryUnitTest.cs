using Chirp.Infrastructure.Repositories;

namespace Chirp.InfrastructureTest.RepositoryTest;

public class FollowRepositoryUnitTest : CoreRepositoryTester
{
    private readonly FollowRepository _followRepository;

    public FollowRepositoryUnitTest() => _followRepository = SetUpFollowRepository().Result;
    
    [Fact]
    public async Task GetFollowersByName()
    {
        //throw new NotImplementedException();
    }
    
    [Fact]
    public async Task GetFollowingByName()
    {
        //throw new NotImplementedException();
    }

    [Fact]
    public async Task GetFollow()
    {
        //throw new NotImplementedException();
    }

    [Fact]
    public async Task AddFollow()
    {
        //throw new NotImplementedException();
    }

    [Fact]
    public async Task RemoveFollow()
    {
        //throw new NotImplementedException();
    }
}