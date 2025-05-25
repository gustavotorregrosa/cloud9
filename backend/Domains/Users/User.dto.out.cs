namespace backend.Domains.Users;

public class UserDTOout
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Active { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}