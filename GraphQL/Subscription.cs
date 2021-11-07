using CommanderGQL.Models;
using HotChocolate;
using HotChocolate.Types;

namespace CommanderGQL.GraphQL
{
  public class Subscription
  {
    [Subscribe]
    [Topic]
    public Platform onPlatformAdded([EventMessage] Platform platform)
    {
      return platform;
    }
  }
}