namespace UserService.DAL.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required Guid AccountId { get; set; }
        public required string TokenHash { get; set; }
        public bool IsActive { get; set; } = true;
        public required DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Account? Account { get; set; }
    }
}
