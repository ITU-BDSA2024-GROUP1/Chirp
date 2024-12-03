using Chirp.Core.DataTransferObject;

namespace Chirp.Infrastructure.Repositories;

public interface IFollowRepository
{
    Task<List<FollowDTO>> GetFollowersByName(string name);
    Task<List<FollowDTO>> GetFollowingByName(string name);
    Task<FollowDTO> GetFollow(string followerName, string followedName);
    Task AddFollow(FollowDTO entity);
    Task RemoveFollow(FollowDTO entity);
}