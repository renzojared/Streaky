using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Streaky.Udemy.DTOs;
using Streaky.Udemy.Services;

namespace Streaky.Udemy.Controllers.V1;

[ApiController]
[Route("appi/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IConfiguration configuration;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IDataProtector dataProtector;
    private readonly HashService hashService;

    public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager, IDataProtectionProvider dataProtectionProvider, HashService hashService)
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.signInManager = signInManager;
        dataProtector = dataProtectionProvider.CreateProtector("unique_value_secret");
        this.hashService = hashService;
    }

    [HttpPost("register", Name = "registerUser")]
    public async Task<ActionResult<ResponseAuthentication>> Register(CredentialUser credentialUser)
    {
        var user = new IdentityUser { UserName = credentialUser.Email, Email = credentialUser.Email };
        var result = await userManager.CreateAsync(user, credentialUser.Password);

        if (result.Succeeded)
        {
            return await BuildToken(credentialUser);
        }
        else
            return BadRequest(result.Errors);
    }

    [HttpPost("login", Name = "loginUser")]
    public async Task<ActionResult<ResponseAuthentication>> Login(CredentialUser credentialUser)
    {
        var result = await signInManager.PasswordSignInAsync(credentialUser.Email, credentialUser.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return await BuildToken(credentialUser);
        else
            return BadRequest("Incorrect login");
    }

    [HttpGet("RenewToken", Name = "renewToken")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ResponseAuthentication>> Renew()
    {
        var emailClaim = HttpContext.User.Claims.Where(c => c.Type == "email").FirstOrDefault();
        var email = emailClaim.Value;
        var credentialUser = new CredentialUser()
        {
            Email = email
        };

        return await BuildToken(credentialUser);
    }

    private async Task<ResponseAuthentication> BuildToken(CredentialUser credentialUser)
    {
        var claims = new List<Claim>()
        {
            new Claim("email", credentialUser.Email),
            new Claim("iwant", "anyelse")
        };

        var user = await userManager.FindByEmailAsync(credentialUser.Email);
        var claimsDb = await userManager.GetClaimsAsync(user);

        claims.AddRange(claimsDb);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyJwty"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expire = DateTime.UtcNow.AddMinutes(30);

        var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expire, signingCredentials: creds);

        return new ResponseAuthentication()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
            Expires = expire
        };
    }

    [HttpPost("RemoveAdmin", Name = "removeAdmin")]
    public async Task<ActionResult> RemoveAdmin(EditAdminDTO editAdminDTO)
    {
        var user = await userManager.FindByEmailAsync(editAdminDTO.Email);
        await userManager.RemoveClaimAsync(user, new Claim("isAdmin", "yes"));
        return NoContent();
    }

    [HttpPost("DoAdmin", Name = "doAdmin")]
    public  async Task<ActionResult> DoAdmin(EditAdminDTO editAdminDTO)
    {
        var user = await userManager.FindByEmailAsync(editAdminDTO.Email);
        await userManager.AddClaimAsync(user, new Claim("isAdmin", "yes"));
        return NoContent();
    }

}

