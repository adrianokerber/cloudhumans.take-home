using CSharpFunctionalExtensions;

namespace CloudHumans.ClaudIA.Domain.Shared.ValueObjects;

public record Role
{
    public string Name { get; }
        
    private Role(string name)
    {
        Name = name.ToUpper();
    }

    public static Result<Role> Create(string roleName)
    {
        var name = roleName.ToUpper();
        if (string.IsNullOrEmpty(name) || !ValidRoleName(name))
            return Result.Failure<Role>("Invalid role name");

        return new Role(name);
    }

    private static bool ValidRoleName(string name)
    {
        return name == Assistant.Name
            || name == User.Name
            || name == System.Name;
    }
        
    public static Role Assistant => new Role("ASSISTANT");
    public static Role User => new Role("USER");
    public static Role System => new Role("SYSTEM");
}