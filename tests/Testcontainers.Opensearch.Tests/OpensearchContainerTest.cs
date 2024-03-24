using OpenSearch.Client;
using OpenSearch.Net;

namespace Testcontainers.Opensearch.Tests;

public class OpensearchContainerTest : IAsyncLifetime
{
  private readonly OpensearchContainer _opensearchContainer = new OpensearchBuilder().Build();

  public Task InitializeAsync()
  {
    return _opensearchContainer.StartAsync();
  }

  public Task DisposeAsync()
  {
    return _opensearchContainer.DisposeAsync().AsTask();
  }

  [Fact]
  [Trait(nameof(DockerCli.DockerPlatform), nameof(DockerCli.DockerPlatform.Linux))]
  public void PingReturnsValidResponse()
  {
    // Given
    var clientSettings = new ConnectionSettings(new Uri(_opensearchContainer.GetConnectionString()));
    clientSettings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);

    var client = new OpenSearchClient(clientSettings);

    // When
    var response = client.Ping();

    // Then
    Assert.True(response.IsValid);
  }
}

