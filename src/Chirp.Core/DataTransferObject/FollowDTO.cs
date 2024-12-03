namespace Chirp.Core.DataTransferObject;

public class FollowDTO : IEquatable<FollowDTO>
{
    public required string FollowerName { get; set; }
    public required string FollowedName { get; set; }

    public bool Equals(FollowDTO that)
    {
        return
            that != null &&
            this.FollowedName == that.FollowedName &&
            this.FollowerName == that.FollowerName;
    }
}