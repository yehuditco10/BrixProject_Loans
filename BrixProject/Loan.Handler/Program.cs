using AutoMapper;
using AutoMapper.Configuration;
using Loan.Api;
using Loan.Data;
using Loan.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace Loan.Handler
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
            Console.Title = "LoanHandler";

            var endpointConfiguration = new EndpointConfiguration("LoanHandler");
            endpointConfiguration.EnableOutbox();
            endpointConfiguration.EnableInstallers();
            //for dev only
            endpointConfiguration.PurgeOnStartup(true);
            var connection = ConfigurationManager.AppSettings["LoansConnection"];
            var connectionOutbox = ConfigurationManager.AppSettings["LoanHandlerOutboxConnection"];
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
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var routing = transport.Routing();
            routing.RouteToEndpoint(
                messageType: typeof(Messages.Commands.ValidateLoan),
                destination: "Rules");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
            containerSettings.ServiceCollection.AddScoped<ILoanService, LoanService>();
            containerSettings.ServiceCollection.AddScoped<ILoanRepository, LoanRepository>();
            containerSettings.ServiceCollection.AddDbContext<LoanContext>(options =>
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
