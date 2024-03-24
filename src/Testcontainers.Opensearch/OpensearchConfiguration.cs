namespace Testcontainers.Opensearch;

/// <inheritdoc cref="ContainerConfiguration" />
[PublicAPI]
public sealed class OpensearchConfiguration : ContainerConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpensearchConfiguration" /> class.
    /// </summary>
    /// <param name="username">The Opensearch username.</param>
    /// <param name="password">The Opensearch password.</param>
    public OpensearchConfiguration(
        string username = null,
        string password = null)
    {
        Username = username;
        Password = password;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpensearchConfiguration" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    public OpensearchConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
        : base(resourceConfiguration)
    {
        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpensearchConfiguration" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    public OpensearchConfiguration(IContainerConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpensearchConfiguration" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    public OpensearchConfiguration(OpensearchConfiguration resourceConfiguration)
        : this(new OpensearchConfiguration(), resourceConfiguration)
    {
        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpensearchConfiguration" /> class.
    /// </summary>
    /// <param name="oldValue">The old Docker resource configuration.</param>
    /// <param name="newValue">The new Docker resource configuration.</param>
    public OpensearchConfiguration(OpensearchConfiguration oldValue, OpensearchConfiguration newValue)
        : base(oldValue, newValue)
    {
        Username = BuildConfiguration.Combine(oldValue.Username, newValue.Username);
        Password = BuildConfiguration.Combine(oldValue.Password, newValue.Password);
    }

    /// <summary>
    /// Gets the Opensearch username.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the Opensearch password.
    /// </summary>
    public string Password { get; }
}
