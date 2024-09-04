using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper )
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){

            var users = await _userRepository.GetMembersAsync();

            return Ok(users);
        }

        
        [HttpGet("{userName}")]
        public async Task<ActionResult<MemberDto>> GetUser([FromRoute] string userName){

            return await _userRepository.GetMemberAsync(userName);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
            
            var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userName == null) return BadRequest("No userName found in token");

            var user = await _userRepository.GetUserByUserNameAsync(userName);

            if (user == null) return BadRequest("Could not find user");

            _mapper.Map(memberUpdateDto, user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update the user");  
        }

    }
}