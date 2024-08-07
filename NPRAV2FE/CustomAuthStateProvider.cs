using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.SessionStorage;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ISessionStorageService _sessionStorage;
    private readonly NavigationManager _navigationManager;
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(ISessionStorageService sessionStorage, NavigationManager navigationManager)
    {
        _sessionStorage = sessionStorage;
        _navigationManager = navigationManager;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userName = await _sessionStorage.GetItemAsync<string>("UserName");
        var userRole = await _sessionStorage.GetItemAsync<string>("UserRole");
        var employeeId = await _sessionStorage.GetItemAsync<string>("EmployeeId");
        var id = await _sessionStorage.GetItemAsync<string>("Id");

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(employeeId) || string.IsNullOrEmpty(id))
        {
            return new AuthenticationState(_anonymous);
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, userRole),
            new Claim("EmployeeId", employeeId),
            new Claim("Id", id)
        }, "apiauth");

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task<string> GetEmployeeIdAsync()
    {
        return await _sessionStorage.GetItemAsync<string>("EmployeeId");
    }

    public async Task<string> GetIdAsync()
    {
        return await _sessionStorage.GetItemAsync<string>("Id");
    }

    public async Task MarkUserAsAuthenticated(string userName, string userRole, string employeeId, string id)
    {
        await _sessionStorage.SetItemAsync("UserName", userName);
        await _sessionStorage.SetItemAsync("UserRole", userRole);
        await _sessionStorage.SetItemAsync("EmployeeId", employeeId);
        await _sessionStorage.SetItemAsync("Id", id);

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, userRole),
            new Claim("EmployeeId", employeeId),
            new Claim("Id", id)
        }, "apiauth");

        var user = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _sessionStorage.RemoveItemAsync("UserName");
        await _sessionStorage.RemoveItemAsync("UserRole");
        await _sessionStorage.RemoveItemAsync("EmployeeId");
        await _sessionStorage.RemoveItemAsync("Id");

        var user = new ClaimsPrincipal(_anonymous);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        var user = new ClaimsPrincipal(_anonymous);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task RedirectIfUserRoleEmpty()
    {
        var userRole = await _sessionStorage.GetItemAsync<string>("UserRole");
        if (string.IsNullOrEmpty(userRole))
        {
            _navigationManager.NavigateTo("/login");
        }
    }
}
