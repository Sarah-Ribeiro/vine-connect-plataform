using System;

namespace ProductService.Services;

public class AuthServiceClient
{

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuthServiceClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var authServiceUrl = _configuration["Services:AuthService:Url"];
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{authServiceUrl}/api/auth/validate-token");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

}
