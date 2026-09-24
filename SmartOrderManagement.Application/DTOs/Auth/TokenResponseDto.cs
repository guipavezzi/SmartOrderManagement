namespace SmartOrderManagement.Application.DTOs.Auth;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
}
