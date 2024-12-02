using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chirp.Core.DataTransferObject;

namespace Chirp.Infrastructure.FollowService;
public interface IFollowService
{
    public Task<List<FollowViewModel>> GetFollowersByName(string name);
    public Task<List<FollowViewModel>> GetFollowingByName(string name);
    public Task<FollowViewModel> GetFollow(string followerName, string followedName);
    public Task AddFollow(FollowViewModel entity);
    public Task RemoveFollow(FollowViewModel entity);
}
