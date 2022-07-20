namespace Azure.Infrastructure.Stacks;

using System.Collections.Immutable;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Azure.Infrastructure.Configuration;
using Azure.Infrastructure.Resources;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public class MainStack : Stack
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
{
    public MainStack()
    {
        if (Configuration is null)
        {
            Configuration = new Configuration();
            Configuration.Validate();
        }

        var eventHubResource = new EventHubResource(
            Configuration,
            location
            );

        var kubernetesResources = new List<KubernetesResource>();
    
        var identityResource = new IdentityResource(
            $"kubernetes",
            Configuration,
            location);

        var resourceGroup = GetResourceGroup("kubernetes", location);
        var kubernetesVirtualNetworkResource = new VirtualNetworkResource(
            "kubernetes",
            Configuration,
            location,
            resourceGroup,
            subnetCount: 1);
        var kubernetesResource = new KubernetesResource(
            $"kubernetes",
            Configuration,
            location,
            resourceGroup,
            monitorResource,
            identityResource,
            kubernetesVirtualNetworkResource.SubnetIds.GetAt(0));
        kubernetesResources.Add(kubernetesResource);
        

        this.KubeConfigs = Output.All(kubernetesResources.Select(x => x.KubeConfig));
        this.KubeFqdns = Output.All(kubernetesResources.Select(x => x.KubeFqdn));
    }

    public static IConfiguration Configuration { get; set; } = default!;

    [Output]
    public Output<ImmutableArray<string>> KubeConfigs { get; private set; }

    [Output]
    public Output<ImmutableArray<string>> KubeFqdns { get; private set; }

    private static ResourceGroup GetResourceGroup(string name, string location) =>
        new(
            $"{Configuration.ApplicationName}-{name}-{location}-{Configuration.Environment}-",
            new ResourceGroupArgs()
            {
                Location = location,
                Tags = Configuration.GetTags(location),
            });
}
