using AutoMapper;
using Messages.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Rules.Api;
using Rules.Data;
using Rules.Services;
using Rules.Services.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Rules.Handler
{
    class Program
    {
        private readonly IConfiguration configuration;
        public Program(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        static async Task Main()
        {
            Console.Title = "Rules";

            var endpointConfiguration = new EndpointConfiguration("Rules");

            endpointConfiguration.EnableOutbox();
            var connection = ConfigurationManager.AppSettings["RulesConnection"];
            var connectionOutbox = ConfigurationManager.AppSettings["RulesOutboxConnection"];
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connectionOutbox);
                });

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString(ConfigurationManager.AppSettings["RabbitMQConnection"]);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var routing = transport.Routing();
            routing.RouteToEndpoint(
                messageType: typeof(ValidateLoan),
                destination: "LoanHandler");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
            containerSettings.ServiceCollection.AddSingleton<IRuleService, RuleService>();
            containerSettings.ServiceCollection.AddScoped<IRuleRepository, RuleRepository>();
            containerSettings.ServiceCollection.AddSingleton<RulesTranslate>(new RulesTranslate()
            {
                Parameters = new Dictionary<string, string>()
                {
                    {"גיל", "Age"},
                    { "יתרה", "Balance" },
                    { "עיר", "City" }

        },
                Signs = new Dictionary<string, string>()
                {
                    {"גדול מ", ">" },
                    { "גדולה מ", ">"},
                    {"קטן מ", "<"},
                    {"קטנה מ", "<"},
                    { "שווה ל", "="}
        }
            });
            containerSettings.ServiceCollection.AddDbContext<RulesContext>(options =>
                        options.UseSqlServer(connection));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            containerSettings.ServiceCollection.AddSingleton(mapper);

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
