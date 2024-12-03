using Chirp.Core.DataTransferObject;
using Chirp.Infrastructure.Repositories;

namespace Chirp.Infrastructure.Services.FollowService;

public class FollowService(IFollowRepository followRepository) : IFollowService
{
    public async Task<List<FollowViewModel>> GetFollowersByName(string name)
    {
        List<FollowDTO> follows = await followRepository.GetFollowersByName(name);
        if (follows == null) return null;
        
        List<FollowViewModel> result = [];
        result.AddRange(follows.Select(FollowDTOToFollowViewModel));

        return result;
    }
    
    public async Task<List<FollowViewModel>> GetFollowingByName(string name)
    {
        List<FollowDTO> follows = await followRepository.GetFollowingByName(name);
        if (follows == null) return null;
        
        List<FollowViewModel> result = [];
        result.AddRange(follows.Select(FollowDTOToFollowViewModel));

        return result;
    }
    
    public async Task<FollowViewModel> GetFollow(string followerName, string followedName)
    {
        FollowDTO follow = await followRepository.GetFollow(followerName, followedName);
        return follow == null ? null : FollowDTOToFollowViewModel(follow);
    }
    
    public async Task AddFollow(FollowViewModel followViewModel)
    {
        FollowDTO follow = FollowViewModelToFollowDTO(followViewModel);
        await followRepository.AddFollow(follow);
    }
    
    public async Task RemoveFollow(FollowViewModel followViewModel)
    {
        FollowDTO follow = FollowViewModelToFollowDTO(followViewModel);
        await followRepository.RemoveFollow(follow);
    }

    private static FollowViewModel FollowDTOToFollowViewModel(FollowDTO followDTO)
    {
        return new(followDTO.FollowerName, followDTO.FollowedName);
    }

    private static FollowDTO FollowViewModelToFollowDTO(FollowViewModel followViewModel)
    {
        return new()
        {
            FollowerName = followViewModel.FollowerName,
            FollowedName = followViewModel.FollowedName
        };
    }
}