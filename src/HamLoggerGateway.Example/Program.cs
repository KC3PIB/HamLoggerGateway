using HamLoggerGateway;
using HamLoggerGateway.Example;
using HamLoggerGateway.MessageProcessors.N1MM;
using HamLoggerGateway.MessageProcessors.N1MM.Schema;
using HamLoggerGateway.MessageProcessors.N1MM.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

#region Setup logging and configuration

// Sets up logging and configuration for the application by initializing the logger, reading the configuration file,
// and ensuring we have at least one server config
using var loggerFactory =
    LoggerFactory.Create(builder =>
        builder.AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.IncludeScopes = true;
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss ";
        }));
var logger = loggerFactory.CreateLogger("HamLoggerGateway.Example");

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json", true, true)
    .Build();

logger.LogInformation("Reading configuration file.");
var config = configuration.Get<HamLoggerGatewaySettings?>();
if (config is { N1MMTcpServer: null, N1MMUdpServer: null })
{
    logger.LogError("Server configuration is incomplete. Please check the configuration settings.");
    return;
}

#endregion

#region Setup N1MM message processor, validators, and handlers

// Initializes the message processor, validators, and handlers for N1MM messages.

var n1mmMessageHandler = new N1MMConsoleOutputMessageHandler();
var n1mmMessageValidators = new Dictionary<Type, IMessageValidator>
{
    { typeof(ContactInfo), new ContactInfoHasBareMinimumValidator() }
};
var n1mmMessageProcessor = new N1MMMessageProcessor(n1mmMessageHandler, n1mmMessageValidators, logger);

#endregion

// Sets up cancellation token and starts the configured servers (TCP or UDP).
var cancellationTokenSource = GenerateCancellationTokenSource();
var servers = new List<ServerBase?>();

try
{
    if (config is { N1MMUdpServer: not null })
    {
        logger.LogInformation("Starting N1MM UDP server at {Server}:{Port}", config.N1MMUdpServer.Address,
            config.N1MMUdpServer.Port);
        servers.Add(new UdpServer(n1mmMessageProcessor, config.N1MMUdpServer, logger));
    }

    if (config is { N1MMTcpServer: not null })
    {
        logger.LogInformation("Starting N1MM TCP server at {Server}:{Port}", config.N1MMTcpServer.Address,
            config.N1MMTcpServer.Port);
        servers.Add(new TcpServer(n1mmMessageProcessor, config.N1MMTcpServer, logger));
    }

    // Start the servers
    foreach (var server in servers) server?.Start(cancellationTokenSource);

    logger.LogInformation("Press CTRL-C to quit.");

    // Wait for cancellation
    await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token);
}
catch (OperationCanceledException)
{
    logger.LogInformation("Operation canceled");
}
catch (Exception ex)
{
    logger.LogError("Unhandled exception: {Message}\n{StackTrace}", ex.Message, ex.StackTrace);
}
finally
{
    foreach (var server in servers) server?.Stop();
}

static CancellationTokenSource GenerateCancellationTokenSource()
{
    var cancellationTokenSource = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) =>
    {
        cancellationTokenSource.Cancel();
        e.Cancel = true;
    };
    return cancellationTokenSource;
}