using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
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

        private readonly IPhotoService _photoService;

        public UserController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
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
            
            
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());

            if (user == null) return BadRequest("Could not find user");

            _mapper.Map(memberUpdateDto, user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update the user");  
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) 
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());

            if (user == null) return BadRequest("Cannot update user");

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync()) return _mapper.Map<PhotoDto>(photo);

            return BadRequest("Problem adding photo");
        }

    }
}