namespace Testcontainers.Opensearch;

/// <inheritdoc cref="ContainerBuilder{TBuilderEntity, TContainerEntity, TConfigurationEntity}" />
[PublicAPI]
public sealed class OpensearchBuilder : ContainerBuilder<OpensearchBuilder, OpensearchContainer, OpensearchConfiguration>
{
    public const string OpensearchVmOptionsDirectoryPath = "/usr/share/opensearch/config/jvm.options.d/";

    public const string OpensearchDefaultMemoryVmOptionFileName = "opensearch-default-memory-vm.options";

    public const string OpensearchDefaultMemoryVmOptionFilePath = OpensearchVmOptionsDirectoryPath + OpensearchDefaultMemoryVmOptionFileName;

    public const string OpensearchImage = "opensearch:2.12.0";

    public const ushort OpensearchHttpsPort = 9200;

    public const ushort OpensearchTcpPort = 9300;
    
    public const ushort OpenSearchDashboardsPort = 5601;

    public const string DefaultUsername = "admin";

    public const string DefaultPassword = "admin";

    //private static readonly byte[] DefaultMemoryVmOption = Encoding.Default.GetBytes(string.Join("\n", "-Xms2147483648", "-Xmx2147483648"));

    /// <summary>
    /// Initializes a new instance of the <see cref="OpensearchBuilder" /> class.
    /// </summary>
    public OpensearchBuilder()
        : this(new OpensearchConfiguration())
    {
        DockerResourceConfiguration = Init().DockerResourceConfiguration;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpensearchBuilder" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    private OpensearchBuilder(OpensearchConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        DockerResourceConfiguration = resourceConfiguration;
    }

    /// <inheritdoc />
    protected override OpensearchConfiguration DockerResourceConfiguration { get; }

    /// <summary>
    /// Sets the Opensearch password.
    /// </summary>
    /// <param name="password">The Opensearch password.</param>
    /// <returns>A configured instance of <see cref="OpensearchBuilder" />.</returns>
    public OpensearchBuilder WithPassword(string password)
    {
        return Merge(DockerResourceConfiguration, new OpensearchConfiguration(password: password))
            .WithEnvironment("OPENSEARCH_INITIAL_ADMIN_PASSWORD", password);
    }

    /// <inheritdoc />
    public override OpensearchContainer Build()
    {
        Validate();
        return new OpensearchContainer(DockerResourceConfiguration);
    }

    /// <inheritdoc />
    protected override OpensearchBuilder Init()
    {
        return base.Init()
            .WithImage(OpensearchImage)
            .WithPortBinding(OpenSearchDashboardsPort, true)
            .WithPortBinding(OpensearchHttpsPort, true)
            .WithPortBinding(OpensearchTcpPort, true)
            .WithUsername(DefaultUsername)
            .WithPassword(DefaultPassword)
            .WithEnvironment("discovery.type", "single-node")
            .WithEnvironment("ingest.geoip.downloader.enabled", "false")
            //.WithResourceMapping(DefaultMemoryVmOption, OpensearchDefaultMemoryVmOptionFilePath)
            .WithWaitStrategy(Wait.ForUnixContainer().AddCustomWaitStrategy(new WaitUntil()));
    }

    /// <inheritdoc />
    protected override void Validate()
    {
        base.Validate();

        _ = Guard.Argument(DockerResourceConfiguration.Password, nameof(DockerResourceConfiguration.Password))
            .NotNull()
            .NotEmpty();
    }

    /// <inheritdoc />
    protected override OpensearchBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new OpensearchConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override OpensearchBuilder Clone(IContainerConfiguration resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new OpensearchConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override OpensearchBuilder Merge(OpensearchConfiguration oldValue, OpensearchConfiguration newValue)
    {
        return new OpensearchBuilder(new OpensearchConfiguration(oldValue, newValue));
    }

    /// <summary>
    /// Sets the Opensearch username.
    /// </summary>
    /// <remarks>
    /// The Docker image does not allow to configure the username.
    /// </remarks>
    /// <param name="username">The Opensearch username.</param>
    /// <returns>A configured instance of <see cref="OpensearchBuilder" />.</returns>
    private OpensearchBuilder WithUsername(string username)
    {
        return Merge(DockerResourceConfiguration, new OpensearchConfiguration(username: username));
    }

    /// <inheritdoc cref="IWaitUntil" />
    private sealed class WaitUntil : IWaitUntil
    {
        private static readonly IEnumerable<string> Pattern = new[] { "\"message\":\"started", "\"message\": \"started\"" };

        /// <inheritdoc />
        public async Task<bool> UntilAsync(IContainer container)
        {
            var (stdout, _) = await container.GetLogsAsync(since: container.StoppedTime, timestampsEnabled: false)
                .ConfigureAwait(false);

            return Pattern.Any(stdout.Contains);
        }
    }
}