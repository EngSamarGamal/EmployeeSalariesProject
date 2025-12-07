using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using Serilog.Events;

namespace App.Infrastructure.Logging;

public static class LoggingConfiguration
{
    public static void AddSerilogLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
        {
            var configuration = context.Configuration;
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //custom SQL columns
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn { ColumnName = "SourceContext", DataType = SqlDbType.NVarChar, DataLength = 128 },
                    new SqlColumn { ColumnName = "UserName", DataType = SqlDbType.NVarChar, DataLength = 256, AllowNull = true },
                    new SqlColumn { ColumnName = "IpAddress", DataType = SqlDbType.NVarChar, DataLength = 50, AllowNull = true },
                    new SqlColumn { ColumnName = "MachineName", DataType = SqlDbType.NVarChar, DataLength = 128, AllowNull = true },
                    new SqlColumn { ColumnName = "UserRole", DataType = SqlDbType.NVarChar, DataLength = 128, AllowNull = true } 
                }
            };

            // logger configuration
            ApplyOverrides(loggerConfiguration.MinimumLevel.Information())
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true // Disable in production
                    },
                    columnOptions: columnOptions
                );
        });
    }

    /// <summary>
    /// Applies log level overrides to reduce noise from system libraries.
    /// </summary>
    private static LoggerConfiguration ApplyOverrides(LoggerConfiguration config)
    {
        return config
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Model.Validation", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Error);
    }

}
