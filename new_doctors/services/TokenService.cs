
using Microsoft.IdentityModel.Tokens;
using new_doctors.models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


public interface ITokenService
{
    string CreateToken(User user);
    ClaimsPrincipal GetPrincipalFromToken(string token);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;


    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }



    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {

        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.FirstName),
        new Claim(ClaimTypes.Email, user.LastName),
        };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value!));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(2),
            signingCredentials: cred
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, 
            ValidateAudience = false, 
            ClockSkew = TimeSpan.Zero 
        };

        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        return principal;
    }

}