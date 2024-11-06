namespace Chirp.Razor;

public static class TestEnvironmentManager
{
    private const string TestEnvVar = "RUNNING_TESTS";
    
    public static bool IsTestEnvironment()
    {
        string? environment = GetTestEnvironmentVariable();
        switch (environment)
        {
            case null:
                SetTestEnvironmentVariable("false");
                return false;
            case "true":
                return true;
            case "false":
                return false;
            default:
                throw new ArgumentException(
                    $"{TestEnvVar} environment variable, was neither true nor false. (Actual: {environment})");
        }
    }
    
    public static void SetIsTestEnvironment(bool value) => SetTestEnvironmentVariable(value.ToString().ToLower());
    
    private static string? GetTestEnvironmentVariable() => GetEnvironmentVariable(TestEnvVar);
    private static void SetTestEnvironmentVariable(string value) => SetEnvironmentVariable(TestEnvVar, value);
    
    private static string? GetEnvironmentVariable(string variable) => Environment.GetEnvironmentVariable(variable);
    private static void SetEnvironmentVariable(string variable, string value) => Environment.SetEnvironmentVariable(variable, value);
}