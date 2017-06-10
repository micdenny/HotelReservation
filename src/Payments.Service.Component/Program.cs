﻿using System;
using System.Threading.Tasks;

namespace Payments.Service.Component
{
    using System.Data.SqlClient;
    using NServiceBus;
    using NServiceBus.Persistence.Sql;

    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Payments.Service.Component";

            var endpointConfiguration = new EndpointConfiguration("Payments.Service.Component");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));

            var connection = @"Data Source=.\SQLEXPRESS;Initial Catalog=HotelReservationsPersistence;Integrated Security=True";
            persistence.SqlVariant(SqlVariant.MsSqlServer);
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });

            endpointConfiguration.UseTransport<MsmqTransport>();

            endpointConfiguration.HeartbeatPlugin(
                serviceControlQueue: "ServiceControl",
                frequency: TimeSpan.FromSeconds(30),
                timeToLive: TimeSpan.FromMinutes(3));

            endpointConfiguration.SendFailedMessagesTo("error");

            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            try
            {
                Console.WriteLine("\r\nBus created and configured; press any key to stop program\r\n");
                Console.ReadKey();
            }
            finally
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }
    }
}
