using AutoMapper;
using BusinessObjects.DTO;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Interface;

namespace ManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository = new UserRepository();
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetAllUsers() => _userRepository.GetAllUsers().Select(_mapper.Map<User, UserDto>).ToList();
        [HttpGet("uid")]
        public ActionResult<UserDto> GetUserById(int uid) => (UserDto)_mapper.Map<UserDto>(_userRepository.GetUserById(uid));

        [HttpPost]
        public IActionResult PostUser(UserDto uDto)
        {
            //set product
            User user = _mapper.Map<User>(uDto);
            //if (_userRepository.FindDuplicateProduct(p.ProductName, p.CategoryId) is not null) return BadRequest();
            _userRepository.InsertUser(user);
            return NoContent();
        }
        [HttpDelete("uid")]
        public IActionResult DeleteUser(int uid)
        {
            var user = _userRepository.GetUserById(uid);
            if (user == null) return NotFound();
            _userRepository.DeleteUser(user);
            return NoContent();
        }
        [HttpPut("uid")]
        public IActionResult UpdateProduct(int uid, UserDto uDto)
        {
            var PTmp = _userRepository.GetUserById(uid);
            if (PTmp == null) return NotFound();
            User u = _mapper.Map<User>(uDto);
            _userRepository.UpdateProduct(u);
            return NoContent();
        }


        //[HttpGet("{email}/{password}")]
        //public IActionResult Login(string email, string password)
        //{

        //    User u = _userRepository.checkLogin(email, password);
        //    if (u == null)
        //    {
        //        return Unauthorized();
        //    }
        //    string role = _userRepository.GetRoleByEmail(email);
        //    var authClaims = new List<Claim>
        //        {
        //            new Claim(JwtRegisteredClaimNames.NameId, u.UserId.ToString()),
        //            new Claim(ClaimTypes.Name, u.Email),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim(ClaimTypes.Role, role)
        //        };
        //    var token = CreateToken(authClaims);
        //    string newToken = new JwtSecurityTokenHandler().WriteToken(token);
        //    return Ok(newToken);
        //}

        //private JwtSecurityToken CreateToken(List<Claim> authClaims)
        //{
        //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["JWT:Issuer"],
        //        audience: _configuration["JWT:Audience"],
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //        );
        //    return token;
        //}
    }
}
