using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.DTOs;

namespace ProAgil.WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    public IConfiguration _config { get; }
    public UserManager<User> _userManager { get; }
    public SignInManager<User> _signInManager { get; }
    public IMapper _mapper { get; }

    public UserController(IConfiguration config,
                          UserManager<User> userManager,
                          SignInManager<User> signInManager,
                          IMapper mapper)
    {
      this._config = config;
      this._userManager = userManager;
      this._signInManager = signInManager;
      this._mapper = mapper;
    }

    [HttpGet("GetUser")]
    public async Task<IActionResult> GetUser()
    {
      return Ok(new User());
    }

    // [HttpPost("Register")]
    // public async Task<IActionResult> Register(UserDTO userDTO)
    // {
    //   try
    //   {
    //     var user = _mapper.Map<User>(userDTO);
    //     var result = await _userManager.CreateAsync(user, userDTO.Password);
    //     var userToReturn = _mapper.Map<UserDTO>(user);
    //   }
    //   catch (System.Exception)
    //   {

    //     throw;
    //   }
    // }
  }
}