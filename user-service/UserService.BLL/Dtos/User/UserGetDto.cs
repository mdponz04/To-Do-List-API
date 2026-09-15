namespace UserService.BLL.Dtos.User
{
    public class UserGetDto
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
