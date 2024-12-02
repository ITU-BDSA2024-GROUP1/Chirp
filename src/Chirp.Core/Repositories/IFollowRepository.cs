using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chirp.Core.DataTransferObject;

namespace Chirp.Core.Repositories;

public interface IFollowRepository
{
    Task<List<FollowDTO>> GetFollowersByName (string namne);
    Task<List<FollowDTO>> GetFollowingByName (string name);
    Task<FollowDTO> GetFollow (string followerName, string followedName);
    Task AddFollow (FollowDTO entity);
    Task RemoveFollow (FollowDTO entity);
}
