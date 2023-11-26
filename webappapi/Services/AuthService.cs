// AuthService.cs
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class AuthService : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<SignInResult> Authenticate(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return SignInResult.Failed;
        }

        return await _signInManager.CheckPasswordSignInAsync(user, password, false);
    }
}