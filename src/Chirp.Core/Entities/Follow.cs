using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core.Entities;

[PrimaryKey(nameof(FollowerId), nameof(FollowedId))]
public class Follow
{
    public required string FollowerId { get; set; } 
    public required Author Follower { get; set; } 
    public required string FollowedId { get; set; }
    public required Author Followed { get; set; }
}