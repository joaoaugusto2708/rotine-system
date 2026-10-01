namespace LifeQuest.Application.Common.Security;

public static class RoleNames
{
    public const string Admin = "Admin";
    public const string Psychologist = "Psychologist";
    public const string Coach = "Coach";
    public const string User = "User";

    public static readonly string[] All =
    [
        Admin,
        Psychologist,
        Coach,
        User
    ];
}