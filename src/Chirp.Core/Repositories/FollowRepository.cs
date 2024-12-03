using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Chirp.Core.Data;
using Chirp.Core.DataTransferObject;
using Chirp.Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Repositories;
public class FollowRepository(ChirpDBContext dbContext) : IFollowRepository
{
    public async Task<List<FollowDTO>> GetFollowersByName(string name)
    {
        var query = dbContext.Follows.Where(a => a.Follower.UserName == name).Select(f => new FollowDTO
        {
            FollowerName = f.Follower.UserName,
            FollowedName = f.Followed.UserName
        });
        if (query == null) return null;

        return await query.ToListAsync();
    }

    public async Task<List<FollowDTO>> GetFollowingByName(string name)
    {
        var query = dbContext.Follows.Where(a => a.Followed.UserName == name).Select(f => new FollowDTO
        {
            FollowerName = f.FollowerId,
            FollowedName = f.FollowerId
        });
        if (query == null) return null;

        return await query.ToListAsync();
    }

    public async Task<FollowDTO> GetFollow(string followerName, string followedName) 
    {
        var followDTO = await dbContext.Follows.Where(a => a.Follower.UserName == followerName && a.Followed.UserName == followedName).Select(f => new FollowDTO
        {
            FollowerName = f.Follower.UserName,
            FollowedName = f.Followed.UserName
        }).FirstOrDefaultAsync();
        if (followDTO == null) return null;
        return followDTO;
    }

    public async Task AddFollow(FollowDTO entity) 
    {
        var follower = await dbContext.Authors.Where(a => a.UserName == entity.FollowerName).FirstOrDefaultAsync();
        var followed = await dbContext.Authors.Where(a => a.UserName == entity.FollowedName).FirstOrDefaultAsync();
        if (follower == null) throw new KeyNotFoundException("Follower not found");
        if (follower == null) throw new KeyNotFoundException("Followed not found");

        var follow = new Follow
        {
            Follower = follower,
            FollowerId = follower.Id,
            Followed = followed,
            FollowedId = followed.Id

        };

        var validationContext = new ValidationContext(follow);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(follow, validationContext, validationResults, true))
        {
            var messages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
            throw new ValidationException(messages);
        }

        var queryResult = await dbContext.Follows.AddAsync(follow);

        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveFollow(FollowDTO entity)
    {
        var follower = await dbContext.Authors.Where(a => a.UserName == entity.FollowerName).FirstOrDefaultAsync();
        var followed = await dbContext.Authors.Where(a => a.UserName == entity.FollowedName).FirstOrDefaultAsync();
        if (follower == null) throw new KeyNotFoundException("Follower not found");
        if (follower == null) throw new KeyNotFoundException("Followed not found");

        var follow = new Follow
        {
            Follower = follower,
            FollowerId = follower.Id,
            Followed = followed,
            FollowedId = followed.Id

        };

        var validationContext = new ValidationContext(follow);
        var validationResults = new List<ValidationResult>();

        if (!Validator.TryValidateObject(follow, validationContext, validationResults, true))
        {
            var messages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
            throw new ValidationException(messages);
        }

        var queryResult = dbContext.Follows.Remove(follow);

        await dbContext.SaveChangesAsync();
    }
}
