using Azure;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
namespace WindyFarm.Gin.ServerLog
{
    public class ConsoleColor
    {
        public static readonly string White = "\x1b[37m";
        public static readonly string WhiteDimmed = "\x1b[37m\x1b[2m";
        public static readonly string Red = "\u001b[31m";
        public static readonly string Green = "\u001b[32m";
        public static readonly string Yellow = "\x1b[33m";
        public static readonly string Magenta = "\x1b[35m";
        public static readonly string Cyan = "\x1b[36m";
        public static readonly string Grey = "\x1b[90m";
        public static readonly string WhiteOnRed = "\x1b[37;41m";
        public static readonly string DarkGrey = "\x1b[90m";

    }
    public class GinLogger
    {
        private static ILogger? _logger;
        
        private static ILogger SetUpLogger()
        {
            var customTheme = new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string>
            {
                [ConsoleThemeStyle.Text] = ConsoleColor.White,
                [ConsoleThemeStyle.SecondaryText] = ConsoleColor.WhiteDimmed, // White dimmed
                [ConsoleThemeStyle.TertiaryText] = ConsoleColor.Cyan, // Cyan for timestamp
                [ConsoleThemeStyle.Invalid] = ConsoleColor.Red, // Red
                [ConsoleThemeStyle.Null] = ConsoleColor.Magenta, // Magenta
                [ConsoleThemeStyle.Name] = ConsoleColor.Cyan, // Cyan
                [ConsoleThemeStyle.String] = ConsoleColor.Green, // Green
                [ConsoleThemeStyle.Number] = ConsoleColor.Yellow, // Yellow
                [ConsoleThemeStyle.Boolean] = ConsoleColor.Yellow, // Yellow
                [ConsoleThemeStyle.Scalar] = ConsoleColor.Yellow, // Yellow
                [ConsoleThemeStyle.LevelVerbose] = ConsoleColor.White, // White
                [ConsoleThemeStyle.LevelDebug] = ConsoleColor.Grey, // Grey
                [ConsoleThemeStyle.LevelInformation] = ConsoleColor.White, // White
                [ConsoleThemeStyle.LevelWarning] = ConsoleColor.Yellow, // Yellow
                [ConsoleThemeStyle.LevelError] = ConsoleColor.Red, // Red
                [ConsoleThemeStyle.LevelFatal] = ConsoleColor.WhiteOnRed, // White text on red background
            });

            _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}]\x1b[37m: {Message:lj}{NewLine}{Exception}",
                theme: customTheme,
                applyThemeToRedirectedOutput: true
            )
            .WriteTo.File(
                "serverlogs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}]: {Message:lj} {NewLine}{Exception}"

            )
            .CreateLogger();

            return _logger;
        }

        private static ILogger GetNonNullLogger()
        {
            _logger ??= SetUpLogger();

            return _logger;
        }

        public static void Print(string msg)
        {
            GetNonNullLogger().Information($">>>>> {msg}");
        }
        public static void Debug(string msg)
        {
            GetNonNullLogger().Debug($"{ConsoleColor.DarkGrey}{msg}");
        }
        public static void Info(string info)
        {
            GetNonNullLogger().Information(info);
        }

        public static void Warning(string mesagge, Exception? exception = null)
        {
            GetNonNullLogger().Warning($"{ConsoleColor.Yellow}{mesagge}", exception);
        }

        public static void Error(Exception exception)
        {
            Error(String.Empty, exception);
        }

        public static void Error(string tag, Exception exception)
        {
            Error($"{tag}{ConsoleColor.Red}{exception}");
        }

        public static void Error(string error)
        {
            GetNonNullLogger().Error(error);
        }

        public static void Fatal(string error)
        {
            GetNonNullLogger().Fatal(error);
        }

        public static void Fatal(Exception exception)
        {
            Fatal(String.Empty, exception);
        }

        public static void Fatal(string tag, Exception exception)
        {
            Fatal($"{tag}{ConsoleColor.Red}{exception}");
        }
    }
}
