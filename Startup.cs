using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommanderGQL.Data;
using CommanderGQL.GraphQL;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL.Platforms;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommanderGQL
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    private readonly IConfiguration _config;
    public Startup(IConfiguration config)
    {
      _config = config;
    }
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddPooledDbContextFactory<AppDbContext>(opt => opt.UseSqlServer(_config.GetConnectionString("DefaultConnection")));
      services
        .AddGraphQLServer()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddSubscriptionType<Subscription>()
        .AddType<PlatformType>()
        .AddType<CommandType>()
        .AddFiltering()
        .AddSorting()
        .AddInMemorySubscriptions();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseWebSockets();
      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGraphQL();
      });
      
      app.UseGraphQLVoyager(new VoyagerOptions()
      {
        GraphQLEndPoint = "/graphql",
      }, "/graphql-voyager");
    }
  }
}
