#pragma warning disable CS8618

namespace Web.ControllerApi.Template.Options;

public class DatabaseConfig
{
    public string DbConnectionString { get; set; }
    public bool EnableRetryOnFailure { get; set; }
    public int MaxRetryCount { get; set; }
    public int MaxRetryDelay { get; set; }
    public ICollection<string> ErrorNumbersToAdd { get; set; }
    public int CommandTimeout { get; set; }
}