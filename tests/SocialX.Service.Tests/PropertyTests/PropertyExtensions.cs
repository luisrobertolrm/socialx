namespace SocialX.Service.Tests.PropertyTests;

/// <summary>
/// Extension methods for FsCheck properties
/// </summary>
public static class PropertyExtensions
{
    public static bool Implies(this bool condition, Func<bool> consequence)
    {
        return !condition || consequence();
    }
}
