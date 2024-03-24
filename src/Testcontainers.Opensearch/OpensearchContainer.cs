namespace Testcontainers.Opensearch;

/// <inheritdoc cref="DockerContainer" />
[PublicAPI]
public sealed class OpensearchContainer : DockerContainer
{
    private readonly OpensearchConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpensearchContainer" /> class.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    public OpensearchContainer(OpensearchConfiguration configuration)
        : base(configuration)
    {
        _configuration = configuration;
    }
    
    
    public string GetConnectionString()
    {
        var endpoint = 
            new UriBuilder(Uri.UriSchemeHttps, Hostname, GetMappedPublicPort(OpensearchBuilder.OpensearchHttpsPort))
            {
                UserName = _configuration.Username,
                Password = _configuration.Password,
            };
        
        return endpoint.ToString();
    }
}
