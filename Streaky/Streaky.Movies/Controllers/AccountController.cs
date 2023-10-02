using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Streaky.Movies.DTOs;

namespace Streaky.Movies.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : CustomBaseController
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IConfiguration configuration;
    private readonly ApplicationDbContext context;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.configuration = configuration;
        this.context = context;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
    {
        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
            return await BuildToken(model);
        else
            return BadRequest(result.Errors);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
    {
        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return await BuildToken(model);
        else
            return BadRequest("Incorrect login");
    }

    [HttpPost("RenewToken")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<UserToken>> Renew()
    {
        var userInfo = new UserInfo
        {
            Email = HttpContext.User.Identity.Name
        };
        return await BuildToken(userInfo);
    }

    private async Task<UserToken> BuildToken(UserInfo userInfo)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userInfo.Email),
            new Claim(ClaimTypes.Email, userInfo.Email),
        };
        var identityUser = await userManager.FindByEmailAsync(userInfo.Email);

        claims.Add(new Claim(ClaimTypes.NameIdentifier, identityUser.Id));

        var claimsDB = await userManager.GetClaimsAsync(identityUser);

        claims.AddRange(claimsDB);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddYears(1);

        JwtSecurityToken token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }

    [HttpGet("Users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<List<UserDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
    {
        var queryable = context.Users.AsQueryable();
        queryable = queryable.OrderBy(s => s.Email);
        return await Get<IdentityUser, UserDTO>(paginationDTO);
    }

    [HttpGet("Roles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<List<string>>> GetRoles()
    {
        return await context.Roles.Select(s => s.Name).ToListAsync();
    }

    [HttpPost("AssignRol")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult> AssignRol(EditRolDTO editRolDTO)
    {
        var user = await userManager.FindByIdAsync(editRolDTO.UserId);
        if (user is null)
            return NotFound();

        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editRolDTO.NameRol));
        return NoContent();
    }

    [HttpPost("RemoveRol")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult> RemoveRol(EditRolDTO editRolDTO)
    {
        var user = await userManager.FindByIdAsync(editRolDTO.UserId);
        if (user is null)
            return NotFound();

        await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editRolDTO.NameRol));
        return NoContent();
    }
}

