using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chirp.Core.DataTransferObject;

namespace Chirp.Core.Repositories;

public interface IFollowRepository
{
    Task<List<FollowDTO>> GetFollowersById (string id);
    Task<List<FollowDTO>> GetFollowingById (string id);
    Task<FollowDTO> GetFollow (string followerId, string followedId);
    Task AddFollow (FollowDTO entity);
    Task RemoveFollow (FollowDTO entity);
}
