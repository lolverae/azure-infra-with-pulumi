namespace Azure.Infrastructure;

using Pulumi;
using Azure.Infrastructure.Stacks;

public static class Program
{
    public static Task<int> Main() => Deployment.RunAsync<MainStack>();
}
