using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chirp.Core.DataTransferObject;
using Chirp.Core.Repositories;

namespace Chirp.Infrastructure.FollowService;
public class FollowService(IFollowRepository followRepository) : IFollowService
{
    async public Task<List<FollowViewModel>> GetFollowersById(string id)
    {
        List<FollowDTO> follows = await followRepository.GetFollowersById(id);
        List<FollowViewModel> result = new List<FollowViewModel>();
        foreach (FollowDTO follow in follows) result.Add(new FollowViewModel(follow.FollowerId, follow.FollowerId));
        return result;
    }
    async public Task<List<FollowViewModel>> GetFollowingById(string id)
    {
        List<FollowDTO> follows = await followRepository.GetFollowingById(id);
        List<FollowViewModel> result = new List<FollowViewModel>();
        foreach (FollowDTO follow in follows) result.Add(new FollowViewModel(follow.FollowerId, follow.FollowerId));
        return result;
    }
    async public Task<FollowViewModel> GetFollow(string followerId, string followedId)
    {
        FollowDTO follow = await followRepository.GetFollow(followerId, followedId);
        return new FollowViewModel(follow.FollowerId, follow.FollowedId);
    }
    public Task AddFollow(FollowViewModel entity)
    {
        FollowDTO follow = new FollowDTO { FollowedId = entity.followedId, FollowerId = entity.followerId };
        followRepository.AddFollow(follow);
        return Task.CompletedTask;
    }
    public Task RemoveFollow(FollowViewModel entity)
    {
        FollowDTO follow = new FollowDTO { FollowedId = entity.followedId, FollowerId = entity.followerId };
        followRepository.RemoveFollow(follow);
        return Task.CompletedTask;
    }
}
