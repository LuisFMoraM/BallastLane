using Authorization;
using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserApi.DTOs;

namespace UserApi.Controllers
{
    /// <summary>
    /// Define Endpoints related to Users
    /// </summary>
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthorizationConfig _authorizationConfig;
        private readonly IUserService _userService;

        public UserController(
            ILogger<UserController> logger,
            IMapper mapper,
            IAuthorizationConfig authorizationConfig,
            IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _authorizationConfig = authorizationConfig;
            _userService = userService;
        }

        /// <summary>
        /// Adds a new User to the system
        /// </summary>
        /// <param name="user">User info</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(UserDto user)
        {
            var errorMsg = $"Could not Add a new User into the system. " +
                $"Sent data: {JsonConvert.SerializeObject(user)}.";

            try
            {
                var record = _mapper.Map<User>(user);
                await _userService.Add(record);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, errorMsg);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMsg);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Allows the user to log in to the system
        /// </summary>
        /// <param name="loginInfo">Information to Log in</param>
        [HttpPost("{email}/login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<IActionResult> Login(string email, LoginDto loginInfo)
        {
            var errorMsg = $"The user could not Log in to the system. " +
                $"Sent data: {JsonConvert.SerializeObject(loginInfo)}.";

            try
            {
                var user = await _userService
                    .LogIn(email, loginInfo.Password);

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Token = _authorizationConfig.JsonWebToken();
                return Ok(userDto);
            }
            catch (Exception ex) when 
                (ex is InvalidOperationException || ex is ArgumentException)
            {
                _logger.LogError(ex, errorMsg);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMsg);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
