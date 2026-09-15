using AutoMapper;
using UserService.BLL.Dtos.User;
using UserService.BLL.Interfaces;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class UserService : IUserService
    {
        public IUserRepo UserRepo { get; set; }
        public IMapper Mapper { get; set; }

        public UserService(IUserRepo userRepo, IMapper mapper)
        {
            UserRepo = userRepo;
            Mapper = mapper;
        }

        public async Task<UserGetDto> Create(UserPostDto userDto)
        {
            return Mapper.Map<UserGetDto>(await UserRepo.Create(Mapper.Map<User>(userDto)));
        }

        public async Task<UserGetDto> Get(Guid userId)
        {
            return Mapper.Map<UserGetDto>(await UserRepo.Get(userId));
        }

        public async Task<bool> HardDelete(Guid userId)
        {
            return await UserRepo.Delete(userId);
        }

        public async Task<bool> SoftDelete(Guid userId)
        {
            var user = await UserRepo.Get(userId);
            if(user == null) return false;
            user.IsDeleted = true;
            return await UserRepo.Update(user) != null;
        }

        public async Task<UserGetDto> Update(Guid userId, UserPutDto userDto)
        {
            var user = Mapper.Map<User>(userDto);
            user.Id = userId;
            return Mapper.Map<UserGetDto>(await UserRepo.Update(user));
        }
    }
}
