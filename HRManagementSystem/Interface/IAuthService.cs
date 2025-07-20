namespace HRManagementSystem.Interface;
using HRManagementSystem.Models.ViewModel;
using HRManagementSystem.Models;


public interface IAuthService
{
    Task<User> RegisterAsync(RegisterViewModel model);
    Task<User?> LoginAsync(LoginViewModel model);

}

