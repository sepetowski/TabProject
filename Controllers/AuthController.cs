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
        private readonly IAuthRepository _authRepository;


        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterReqDTO request)
        {

            try
            {
                var newUser = await _authRepository.CreateNewUserAsync(request);
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
                var user = await _authRepository.LoginUserAsync(request);
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
            var res = await _authRepository.GenerateRefreshTokenAsync(refreshTokenDTO);
            if (res == null)
                return Unauthorized();

            return Ok(res);
        }
    }
}
