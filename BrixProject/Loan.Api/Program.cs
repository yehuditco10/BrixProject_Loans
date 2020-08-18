using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System.IO;
using Loan.Api;

namespace Loans.Api
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
     .Build();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
  .UseNServiceBus(context =>
  {
      var endpointConfiguration = new EndpointConfiguration("Loan");
      endpointConfiguration.PurgeOnStartup(true);
      endpointConfiguration.EnableInstallers();
      var outboxSettings = endpointConfiguration.EnableOutbox();
      var recoverability = endpointConfiguration.Recoverability();
      recoverability.Delayed(
          customizations: delayed =>
          {
              delayed.NumberOfRetries(2);
              delayed.TimeIncrease(TimeSpan.FromMinutes(4));
          });
      recoverability.Immediate(
          customizations: immediate =>
          {
              immediate.NumberOfRetries(3);

          });

      var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
      transport.UseConventionalRoutingTopology()
      .ConnectionString(Configuration.GetConnectionString("RabbitMQConnection"));
      var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
      var connectionOutbox = Configuration.GetConnectionString("LoanOutboxConnection");
      persistence.SqlDialect<SqlDialect.MsSqlServer>();
      persistence.ConnectionBuilder(
          connectionBuilder: () =>
          {
              return new SqlConnection(connectionOutbox);
          });

      var routing = transport.Routing();
      routing.RouteToEndpoint(
          messageType: typeof(Messages.Commands.ValidateLoan),
          destination: "LoanHandler");

      endpointConfiguration.SendFailedMessagesTo("error");
      endpointConfiguration.AuditProcessedMessagesTo("audit");
      var conventions = endpointConfiguration.Conventions();
      conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
      conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");


      return endpointConfiguration;
  })
   .ConfigureWebHostDefaults(webBuilder =>
   {
       webBuilder.UseStartup<Startup>()
                 .UseConfiguration(Configuration);
   });

    }
}

