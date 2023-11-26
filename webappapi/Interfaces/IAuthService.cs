// IAuthService.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<SignInResult> Authenticate(string username, string password);
}