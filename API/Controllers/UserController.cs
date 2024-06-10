using API.DTOs;
using API.Interfaces;
using API.Models;
using API.Services;
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

    }
}