using AutoMapper;
using UserService.BLL.Dtos.Account;
using UserService.BLL.Dtos.Permission;
using UserService.BLL.Dtos.Role;
using UserService.BLL.Dtos.User;
using UserService.DAL.Entities;

namespace UserService.BLL.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User mappings
            CreateMap<User, UserGetDto>().ReverseMap();
            CreateMap<User, UserPostDto>().ReverseMap();
            CreateMap<User, UserPutDto>().ReverseMap();
            //Account mappings
            CreateMap<Account, AccountGetDto>().ReverseMap();
            CreateMap<Account, AccountPostDto>().ReverseMap();
            CreateMap<Account, AccountPutDto>().ReverseMap();
            //Role mappings
            CreateMap<Role, RoleGetDto>().ReverseMap();
            CreateMap<Role, RolePostDto>().ReverseMap();
            CreateMap<Role, RolePutDto>().ReverseMap();
            //Permission mappings
            CreateMap<Permission, PermissionGetDto>().ReverseMap();
            CreateMap<Permission, PermissionPostDto>().ReverseMap();
            CreateMap<Permission, PermissionPutDto>().ReverseMap();
        }
    }
}
