using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Entities;

[PrimaryKey(nameof(FollowerId), nameof(FollowedId))]
public class Follow
{
    public string FollowerId { get; set; } 
    public required Author Follower { get; set; } 
    public string FollowedId { get; set; }
    public required Author Followed { get; set; }
}