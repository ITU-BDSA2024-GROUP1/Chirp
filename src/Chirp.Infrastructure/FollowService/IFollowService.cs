using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chirp.Core.DataTransferObject;

namespace Chirp.Infrastructure.FollowService;
public interface IFollowService
{
    public Task<List<FollowViewModel>> GetFollowersById(string id);
    public Task<List<FollowViewModel>> GetFollowingById(string id);
    public Task<FollowViewModel> GetFollow(string followerId, string followedId);
    public Task AddFollow(FollowViewModel entity);
    public Task RemoveFollow(FollowViewModel entity);
}
