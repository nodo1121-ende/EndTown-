namespace EndTown.Models.Entities
{
    public enum FriendshipStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public class FriendShips
    {
        public int Id { get; set; }

        public FriendshipStatus Status { get; set; } = FriendshipStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    
        public int SenderId { get; set; }
        public User Sender { get; set; } = null!;

       
       
        public int ReceiverId { get; set; }
        public User Receiver { get; set; } = null!;
    }
}