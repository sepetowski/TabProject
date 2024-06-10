using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.DTO.Auth;

namespace TabProjectServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthSerivce _authService;


        public AuthController(IAuthSerivce authService)
        {
            _authService = authService;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterReqDTO request)
        {

            try
            {
                var newUser = await _authService.CreateNewUserAsync(request);
                return Ok(newUser);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReqDTO request)
        {

            try
            {
                var user = await _authService.LoginUserAsync(request);
                return Ok(user);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }


        [HttpPost("refreshToken")]
        public async Task<IActionResult> GenerateRefreshToken([FromBody] RefreshTokenReqDTO refreshTokenDTO)
        {
            var res = await _authService.GenerateRefreshTokenAsync(refreshTokenDTO);
            if (res == null)
                return Unauthorized();

            return Ok(res);
        }
    }
}
