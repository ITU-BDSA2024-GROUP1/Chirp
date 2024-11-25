namespace Chirp.Core.DataTransferObject;

public class FollowDTO : IEquatable<FollowDTO>
{
    public required string FollowerId { get; set; }
    public required string FollowedId { get; set; }

    public bool Equals(FollowDTO that)
    {
        return
            that != null &&
            FollowedId.Equals(that.FollowedId) &&
            FollowerId.Equals(that.FollowerId);
    }
}