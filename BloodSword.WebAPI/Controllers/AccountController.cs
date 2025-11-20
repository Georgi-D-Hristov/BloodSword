using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// ПРОМЕНИ: Този DTO namespace е грешен. Трябва да го оправиш:
// using BloodSword.Application.DTOs; // <-- ТРЯБВА ТИ ТОВА
using BloodSword.Application.Contracts;
using BloodSword.Application.DTOs; // Трябва за UserRoles

namespace BloodSword.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        // DI за управление на потребителите и достъп до настройките (конфигурацията)
        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // POST: api/Account/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // ЗАБЕЛЕЖКА: Трябва да дефинираш RegisterModel в Domain/DTOs, ако не си го направил
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            // Ако регистрацията е успешна, автоматично му даваме роля Player
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Player);
                return Ok(new { Status = "Success", Message = "User created and assigned Player role successfully!" });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed!" });
        }


        // POST: api/Account/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // ЗАБЕЛЕЖКА: Трябва да дефинираш LoginModel в Domain/DTOs
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // 1. Дефинираме claims (информация, която ще влезе в токена)
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                // Добавяме ролите като claims
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // 2. Създаваме токена
                // ВАЖНО: Ключът трябва да е същият като в Program.cs!
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TovaEmnogoDylgaTajnaParolaZaTokenite123!"));

                var token = new JwtSecurityToken(
                    issuer: null, // Поради конфигурацията ни
                    audience: null,
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                // 3. Връщаме токена
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
    }
}