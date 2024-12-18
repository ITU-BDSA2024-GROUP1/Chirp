using Chirp.Infrastructure.Repositories;
using Chirp.Infrastructure.Services.FollowService;

namespace Chirp.InfrastructureTest.ServiceTest;

public class FollowServiceUnitTest : InfrastructureServiceTester
{
    private readonly FollowService _followService;

    public FollowServiceUnitTest()
    {
        FollowRepository followRepository = new(_context);
        _followService = new(followRepository);
    }

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