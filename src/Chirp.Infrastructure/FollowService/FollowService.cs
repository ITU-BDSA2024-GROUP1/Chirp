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
    async public Task<List<FollowViewModel>> GetFollowersByName(string name)
    {
        List<FollowDTO> follows = await followRepository.GetFollowersByName(name);
        if (follows == null) return null;
        List<FollowViewModel> result = new List<FollowViewModel>();
        foreach (FollowDTO follow in follows) result.Add(new FollowViewModel(follow.FollowerName, follow.FollowerName));
        return result;
    }
    async public Task<List<FollowViewModel>> GetFollowingByName(string name)
    {
        List<FollowDTO> follows = await followRepository.GetFollowingByName(name);
        if (follows == null) return null;
        List<FollowViewModel> result = new List<FollowViewModel>();
        foreach (FollowDTO follow in follows) result.Add(new FollowViewModel(follow.FollowerName, follow.FollowerName));
        return result;
    }
    async public Task<FollowViewModel> GetFollow(string followerName, string followedName)
    {
        FollowDTO follow = await followRepository.GetFollow(followerName, followedName);
        if (follow == null) return null;
        return new FollowViewModel(follow.FollowerName, follow.FollowedName);
    }
    public Task AddFollow(FollowViewModel entity)
    {
        FollowDTO follow = new FollowDTO { FollowedName = entity.followedName, FollowerName = entity.followerName };
        followRepository.AddFollow(follow);
        return Task.CompletedTask;
    }
    public Task RemoveFollow(FollowViewModel entity)
    {
        FollowDTO follow = new FollowDTO { FollowedName = entity.followedName, FollowerName = entity.followerName };
        followRepository.RemoveFollow(follow);
        return Task.CompletedTask;
    }
}
