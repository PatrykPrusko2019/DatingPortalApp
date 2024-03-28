using API.Data;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<AppUser>> GetUsers(){
            var users = await _userService.GetUsers();

            return users;
        }

        
        [HttpGet("{id}")]
        public async Task<AppUser> GetUser([FromRoute] int id){
            var user = await _userService.GetUserById(id);

            return user;
        }

    }
}