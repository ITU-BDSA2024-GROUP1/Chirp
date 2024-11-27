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
    public async Task<List<FollowDTO>> GetFollowersById(string id)
    {
        var query = dbContext.Follows.Where(a => a.FollowerId == id).Select(f => new FollowDTO
        {
            FollowerId = f.FollowerId,
            FollowedId = f.FollowerId
        });
        if (query == null) return null;

        return await query.ToListAsync();
    }

    public async Task<List<FollowDTO>> GetFollowingById(string id)
    {
        var query = dbContext.Follows.Where(a => a.FollowedId == id).Select(f => new FollowDTO
        {
            FollowerId = f.FollowerId,
            FollowedId = f.FollowerId
        });
        if (query == null) return null;

        return await query.ToListAsync();
    }

    public async Task<FollowDTO> GetFollow(string followerId, string followedId) 
    {
        var follow = await dbContext.Follows.Where(a => a.FollowerId == followerId && a.FollowedId == followedId).FirstOrDefaultAsync();
        if (follow != null) return null;
        var followDTO = new FollowDTO { FollowedId = follow.FollowedId, FollowerId = follow.FollowerId };
        return followDTO;
    }

    public async Task AddFollow(FollowDTO entity) 
    {
        var follower = await dbContext.Authors.Where(a => a.Id == entity.FollowerId).FirstOrDefaultAsync();
        var followed = await dbContext.Authors.Where(a => a.Id == entity.FollowedId).FirstOrDefaultAsync();
        if (follower == null) throw new KeyNotFoundException("Follower not found");
        if (follower == null) throw new KeyNotFoundException("Followed not found");

        var follow = new Follow
        {
            Follower = followed,
            FollowerId = entity.FollowerId,
            Followed = followed,
            FollowedId = entity.FollowedId

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
        var follower = await dbContext.Authors.Where(a => a.Id == entity.FollowerId).FirstOrDefaultAsync();
        var followed = await dbContext.Authors.Where(a => a.Id == entity.FollowedId).FirstOrDefaultAsync();
        if (follower != null) throw new KeyNotFoundException("Follower not found");
        if (follower != null) throw new KeyNotFoundException("Followed not found");

        var follow = new Follow
        {
            Follower = followed,
            FollowerId = entity.FollowerId,
            Followed = followed,
            FollowedId = entity.FollowedId

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
