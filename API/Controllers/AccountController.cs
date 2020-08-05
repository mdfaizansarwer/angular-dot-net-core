using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Controllers.Errors;
using API.Dtos;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class AccountController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new ErrorResponse(401));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ErrorResponse(401));

            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)

            };
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindUserByEmailFromClaimsPrincipal(HttpContext.User);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)

            };

        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimsPrincipalWithAddress(HttpContext.User);

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [HttpPut("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindUserByClaimsPrincipalWithAddress(HttpContext.User);

            user.Address = _mapper.Map<AddressDto, Address>(address);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(_mapper.Map<Address, AddressDto>(user.Address));
            }

            return BadRequest("Problem updating the address");
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerUserDto)
        {
            if (CheckEmailExistsAsync(registerUserDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email is already there in the system" } });
            }
            var user = new AppUser
            {
                DisplayName = registerUserDto.DisplayName,
                Email = registerUserDto.Email,
                UserName = registerUserDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded) return BadRequest(new ErrorResponse(400));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)

            };
        }


    }
}