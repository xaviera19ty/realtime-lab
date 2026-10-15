namespace RealtimeLab.Api.Data.Entities;

/// <summary>帳號。寫入方：api。core 只讀（驗證權限時）。</summary>
public class User
{
    public long Id { get; set; }
    public required string Email { get; set; }

    /// <summary>BCrypt 雜湊；只用 OAuth 登入的帳號為 null。</summary>
    public string? PasswordHash { get; set; }

    public required string Name { get; set; }
    public string Role { get; set; } = UserRoles.User;
    public bool IsGuest { get; set; }
    public bool IsDemo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public static class UserRoles
{
    public const string User = "user";
    public const string Admin = "admin";
}
