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

namespace Streaky.Udemy.Controllers;

[ApiController]
[Route("appi/[controller]")]
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

    [HttpGet("encrypt")]
    public ActionResult Encrypt()
    {
        var texPlain = "Renzo Leon";
        var textCrypt = dataProtector.Protect(texPlain);
        var textDescrypt = dataProtector.Unprotect(textCrypt);

        return Ok(new
        {
            texPlain = texPlain,
            textCrypt = textCrypt,
            textDescrypt = textDescrypt
        });
    }

    [HttpGet("encryptByTime")]
    public ActionResult EncryptByTime()
    {
        var protectLimitedByTime = dataProtector.ToTimeLimitedDataProtector();

        var texPlain = "Renzo Leon";
        var textCrypt = protectLimitedByTime.Protect(texPlain, lifetime: TimeSpan.FromSeconds(5));
        Thread.Sleep(6000);
        var textDescrypt = protectLimitedByTime.Unprotect(textCrypt);

        return Ok(new
        {
            texPlain = texPlain,
            textCrypt = textCrypt,
            textDescrypt = textDescrypt
        });
    }

    [HttpGet("hash/{textPlain}")]
    public ActionResult DoHash(string textPlain)
    {
        var result1 = hashService.Hash(textPlain);
        var result2 = hashService.Hash(textPlain);

        return Ok(new
        {
            textPlain = textPlain,
            Hash1 = result1,
            Hash2 = result2
        });
    }

    [HttpPost("register")]
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

    [HttpPost("login")]
    public async Task<ActionResult<ResponseAuthentication>> Login(CredentialUser credentialUser)
    {
        var result = await signInManager.PasswordSignInAsync(credentialUser.Email, credentialUser.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return await BuildToken(credentialUser);
        else
            return BadRequest("Incorrect login");
    }

    [HttpGet("RenewToken")]
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

    [HttpPost("RemoveAdmin")]
    public async Task<ActionResult> RemoveAdmin(EditAdminDTO editAdminDTO)
    {
        var user = await userManager.FindByEmailAsync(editAdminDTO.Email);
        await userManager.RemoveClaimAsync(user, new Claim("isAdmin", "yes"));
        return NoContent();
    }

    [HttpPost("DoAdmin")]
    public  async Task<ActionResult> DoAdmin(EditAdminDTO editAdminDTO)
    {
        var user = await userManager.FindByEmailAsync(editAdminDTO.Email);
        await userManager.AddClaimAsync(user, new Claim("isAdmin", "yes"));
        return NoContent();
    }

}

