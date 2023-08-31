using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using VerifyEmailForgotPassword.Controllers.Models;
using VerifyEmailForgotPassword.Data;

namespace VerifyEmailForgotPassword.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (_context.Users.Any(x => x.Email == request.Email))
            {
                return BadRequest("User Already Exist.");

            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = CreateRandomToken()
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User Successfully Created");

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (user == null) { return BadRequest("User Not Found."); }
            if (user.VerifiedAt == null) { return BadRequest("Not Verified"); }
            if (!VerifyPasswordHash(request.Password,user.PasswordHash,user.PasswordSalt)) { return BadRequest("User Password is Incorrect"); }
            return Ok($"Welcome Back {user.Email}");
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
               
               var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
