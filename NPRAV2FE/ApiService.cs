using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ApiService
{
	private readonly HttpClient _httpClient;

	public ApiService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}


    public async Task<HttpResponseMessage> Login(HttpContent content)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:5267/api/Login", content);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Login: {ex.Message}");
            throw;
        }
    }

    public async Task<HttpResponseMessage> ForgotPassword(StringContent content)
    {
        try
        {
            var response = await _httpClient.PostAsync("https://localhost:5267/api/ForgotPassword", content);
            return response;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }


    public async Task<string> AddProject(HttpContent content)
    {
        var response = await _httpClient.PostAsync("https://localhost:5267/api/AddProject", content);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            return null;
        }
    }


    public async Task<string> ProjectDetails(int projectId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://localhost:5267/api/ProjectDetails/{projectId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching project details: {ex.Message}");
            throw;
        }
    }

    public async Task<string> EditProject(int id, StringContent content)
    {
        var response = await _httpClient.PutAsync($"https://localhost:5267/api/EditProject/{id}", content);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetHomePageProjectsAsync()
    {
        var response = await _httpClient.GetAsync("https://localhost:5267/api/HomePage");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> DeleteProject(int projectId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:5267/api/DeleteProject/{projectId}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error in DeleteProject: {ex.Message}");
            return false;
        }
    }


    public async Task<string> UserAdd(string requestBody = null)
    {
        HttpContent content = requestBody != null ? new StringContent(requestBody, Encoding.UTF8, "application/json") : null;
        HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:5267/api/AddUser", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // Handle error case
            var errorResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error response: {errorResponse}");
            return null;
        }
    }



    public async Task<string> GetUserDetails(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://localhost:5267/api/UserDetails/{userId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user details: {ex.Message}");
            throw;
        }
    }


    public async Task<string> EditUser(int userId, string requestBody)
    {
        try
        {
            var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:5267/api/EditUser/{userId}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP error editing user: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing user: {ex.Message}");
            throw;
        }
    }




    public async Task<bool> UserDelete(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:5267/api/DeleteUser/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting user: {ex.Message}");
            return false;
        }
    }

    public async Task<string> ProfileAdd(string requestBody = null)
	{
		HttpContent content = requestBody != null ? new StringContent(requestBody) : null;
		HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:5267/api/ProfileAdd", content);

		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadAsStringAsync();
		}
		else
		{
			// Handle error case
			return null;
		}
	}

    public async Task<string> GetProfileDetails(int id)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:5267/api/ProfileDetails/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetProfileDetails: {ex.Message}");
            return null;
        }
    }
    public async Task<string> ProfileEdit(int id, string requestBody)
    {
        try
        {
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:5267/api/EditProfile/{id}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error in ProfileEdit: {ex.Message}");
            return null;
        }
    }



    public async Task<string> ProfileDelete(string requestBody = null)
	{
		HttpResponseMessage response = await _httpClient.DeleteAsync("https://localhost:5267/api/ProfileDelete");

		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadAsStringAsync();
		}
		else
		{
			// Handle error case
			return null;
		}
	}
    public async Task<UserRoleModel> GetUserRole()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:5267/api/UserRole");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserRoleModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        else
        {
            return new UserRoleModel { Role = "User" }; // Default role if error occurs
        }
    }
    public async Task<string> GetUserListAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://localhost:5267/api/UserList");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user list: {ex.Message}");
            throw;
        }
    }


    public class UserRoleModel
    {
        public string Role { get; set; }
    }
}
