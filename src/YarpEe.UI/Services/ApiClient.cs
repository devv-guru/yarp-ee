using System.Net.Http.Json;
using System.Text.Json;

namespace YarpEe.UI.Services;

/// <summary>
/// Adapter implementation for HTTP API communication using HttpClient
/// </summary>
public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiClient> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            _logger.LogInformation("GET {Endpoint}", endpoint);
            var response = await _httpClient.GetAsync(endpoint);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("GET {Endpoint} failed with status {StatusCode}", endpoint, response.StatusCode);
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during GET {Endpoint}", endpoint);
            throw;
        }
    }

    public async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            _logger.LogInformation("POST {Endpoint}", endpoint);
            var response = await _httpClient.PostAsJsonAsync(endpoint, data, _jsonOptions);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("POST {Endpoint} failed with status {StatusCode}: {Error}", 
                    endpoint, response.StatusCode, errorContent);
                throw new HttpRequestException($"Request failed: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during POST {Endpoint}", endpoint);
            throw;
        }
    }

    public async Task<T?> PutAsync<T>(string endpoint, object data)
    {
        try
        {
            _logger.LogInformation("PUT {Endpoint}", endpoint);
            var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonOptions);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("PUT {Endpoint} failed with status {StatusCode}: {Error}", 
                    endpoint, response.StatusCode, errorContent);
                throw new HttpRequestException($"Request failed: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during PUT {Endpoint}", endpoint);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string endpoint)
    {
        try
        {
            _logger.LogInformation("DELETE {Endpoint}", endpoint);
            var response = await _httpClient.DeleteAsync(endpoint);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("DELETE {Endpoint} failed with status {StatusCode}", endpoint, response.StatusCode);
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during DELETE {Endpoint}", endpoint);
            throw;
        }
    }
}
