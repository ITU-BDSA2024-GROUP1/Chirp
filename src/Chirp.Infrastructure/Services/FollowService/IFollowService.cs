namespace Chirp.Infrastructure.Services.FollowService;

public interface IFollowService
{
    public Task<List<FollowViewModel>> GetFollowersByName(string name);
    public Task<List<FollowViewModel>> GetFollowingByName(string name);
    public Task<FollowViewModel> GetFollow(string followerName, string followedName);
    public Task AddFollow(FollowViewModel followViewModel);
    public Task RemoveFollow(FollowViewModel entity);
}