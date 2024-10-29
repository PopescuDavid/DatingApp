using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsersAsync()
    {
        var users = await userRepository.GetMembmersAsync();

        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDTO>> GetUserAsync(string username)
    {
        var user = await userRepository.GetMemberAsync(username);
        
        if(user == null){
            return NotFound();
        }

        return user;
    }
}
