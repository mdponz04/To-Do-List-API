using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Models;
using UserService.BLL.Dtos.User;
using UserService.BLL.Interfaces;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserPostDto userDto)
        {
            var createdUser = await _userService.Create(userDto);
            var response = new ResponseModel<UserGetDto>(
                StatusCodes.Status201Created,
                "User successfully created.",
                createdUser
            );
            return Ok(response);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var user = await _userService.Get(userId);
            if (user == null)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "User not found."
                ));
            }

            var response = new ResponseModel<UserGetDto>(
                StatusCodes.Status200OK,
                "User retrieved successfully.",
                user
            );
            return Ok(response);
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> Update(Guid userId, [FromBody] UserPutDto userDto)
        {
            try
            {
                var updatedUser = await _userService.Update(userId, userDto);
                var response = new ResponseModel<UserGetDto>(
                    StatusCodes.Status200OK,
                    "User successfully updated.",
                    updatedUser
                );
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    ex.Message
                ));
            }
        }

        [HttpDelete("hard/{userId:guid}")]
        public async Task<IActionResult> HardDelete(Guid userId)
        {
            var result = await _userService.HardDelete(userId);
            if (!result)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "User not found."
                ));
            }

            var response = new ResponseModel<string>(
                StatusCodes.Status204NoContent,
                "User successfully deleted."
            );
            return Ok(response);
        }

        [HttpDelete("soft/{userId:guid}")]
        public async Task<IActionResult> SoftDelete(Guid userId)
        {
            var result = await _userService.SoftDelete(userId);
            if (!result)
            {
                return NotFound(new ResponseModel<string>(
                    StatusCodes.Status404NotFound,
                    "User not found."
                ));
            }

            var response = new ResponseModel<string>(
                StatusCodes.Status204NoContent,
                "User successfully soft deleted."
            );
            return Ok(response);
        }
    }
}
