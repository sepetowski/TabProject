using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TabProjectServer.Data;
using TabProjectServer.Interfaces;
using TabProjectServer.Models.Domain;
using TabProjectServer.Models.DTO.Auth;

namespace TabProjectServer.Repositories
{
    public class AuthRepository :IAuthRepository
    {
        private readonly IConfiguration _config;
        private readonly DataContext _context;

        public AuthRepository(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;

        }

       public async Task<UserRegisterResDTO?> CreateNewUserAsync(UserRegisterReqDTO req)
        {

           


                string username = req.Username;
                string email = req.Email;



                var existEmail = await _context.Users.FirstOrDefaultAsync((user) => user.Email == email);
                if (existEmail != null)
                {
                    throw new Exception("This email exit");
                }

                var existUsername = await _context.Users.FirstOrDefaultAsync((user) => user.Username == username);
                if (existUsername != null)
                {

                    throw new Exception("This username is already taken");
                }

                Role role;

                if (req.Role != null && req.Role == UserRole.Admin)
                {
                    if (req.AdminRoleKey == null) throw new Exception("Admin role key is required");

                    var adminRole = await _context.Roles.FirstOrDefaultAsync(role => role.RoleKey == req.AdminRoleKey);

                    if (adminRole == null) throw new Exception("Invalid admin role key");

                    role = adminRole;

                }
                else
                {
                    var userRole = await _context.Roles.FirstOrDefaultAsync(role => role.UserRole == UserRole.User);

                    role = userRole!;
                }
            

                CreatePasswordHash(req.Password, out byte[] passwordHash, out byte[] passwordSalt);





                var user = new User
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Username = req.Username,
                    Email = req.Email,
                    Role = role,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var newUserResponse = new UserRegisterResDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,

                };





                return newUserResponse;
            
           
        }

        public async Task<RefreshTokenResDTO?> GenerateRefreshTokenAsync(RefreshTokenReqDTO req)
        {
            var principal = GetTokenPrincipal(req.Token);
            var username = principal?.Identity?.Name;

            if (username == null)
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync((user => user.Username == username));

            if (user is null || user.RefreshToken != req.RefreshToken || user.RefreshTokenExpires < DateTime.Now)
                return null;

            var tokenExpires = DateTime.Now.AddMinutes(60);
            string token = GenerateJwtToken(tokenExpires, user.Username, user.Email);

            string refreshToken = CreateRandomToken();

            var refresTokenExpires = DateTime.Now.AddDays(1);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpires = refresTokenExpires;

            await _context.SaveChangesAsync();

            var res = new RefreshTokenResDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                TokenExpires = tokenExpires,
                RefreshTokenExpires = refresTokenExpires


            };

            return res;
        }

        public async Task<UserLoginResDTO?> LoginUserAsync(UserLoginReqDTO req)
        {
            string username = req.Username;

            var userExist = await _context.Users.FirstOrDefaultAsync((user) => user.Username == username);

            if (userExist == null)
            {
                throw new Exception("User not found");
            }


            if (!VerifyPasswordHash(req.Password, userExist.PasswordHash, userExist.PasswordSalt))
            {
                throw new Exception("Password is incorrect.");
            }

            var tokenExpires = DateTime.Now.AddMinutes(60);
            string token = GenerateJwtToken(tokenExpires, userExist.Username, userExist.Email);

            string refreshToken = CreateRandomToken();

            var refresTokenExpires = DateTime.Now.AddDays(1);

            userExist.RefreshToken = refreshToken;
            userExist.RefreshTokenExpires = refresTokenExpires;



            await _context.SaveChangesAsync();

            var newUserResponse = new UserLoginResDTO
            {
                Id = userExist.Id,
                Email = userExist.Email,
                Username = userExist.Username,
                FirstName = userExist.FirstName,
                LastName = userExist.LastName,
                JSONWebToken = token,
                JSONWebTokenExpires = tokenExpires,
                RefreshToken = refreshToken,
                RefreshTokenExpires = refresTokenExpires,
                Role= userExist.Role.UserRole,

            };

            return newUserResponse;

        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }



        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        private string GenerateJwtToken(DateTime tokenExpires, string username, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name,username),
                new (ClaimTypes.Email,email)
            };

            var secToken = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              null,
              expires: tokenExpires,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(secToken);


        }

  

        private ClaimsPrincipal? GetTokenPrincipal(string token)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = securityKey,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
    }
}

