using Serilog.Core;
using Serilog;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddHostedService<Worker>();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(BuildLogger());

            var host = builder.Build();
            host.Run();
        }

        private static void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.AddSerilog(BuildLogger());
        }

        private static Logger BuildLogger()
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd-HHmm");
            var currentPath = AppContext.BaseDirectory;
            var logPath = Path.Combine(currentPath, "logs", $"{timestamp}.log");

            Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            var template = "[{Timestamp:HH:mm:ss}][{Level:u3}] {Message:lj}{NewLine}";

            var logger = new LoggerConfiguration()
                            .WriteTo.Console(outputTemplate: template)
                            .WriteTo.File(logPath, outputTemplate: template)
                            .MinimumLevel.Information()
                            .CreateLogger();

            return logger;
        }
    }
}