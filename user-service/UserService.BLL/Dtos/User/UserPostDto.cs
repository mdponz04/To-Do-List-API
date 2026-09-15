namespace UserService.BLL.Dtos.User
{
    public class UserPostDto
    {
        public Guid AccountId { get; set; }
        public required string UserName { get; set; }
    }
}
