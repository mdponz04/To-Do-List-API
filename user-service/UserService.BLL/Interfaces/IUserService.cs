using UserService.BLL.Dtos.User;

namespace UserService.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserGetDto> Create(UserPostDto userDto);
        public Task<UserGetDto> Get(Guid userId);
        public Task<UserGetDto> Update(Guid userId, UserPutDto userDto);
        public Task<bool> SoftDelete(Guid userId);
        public Task<bool> HardDelete(Guid userId);
    }
}
