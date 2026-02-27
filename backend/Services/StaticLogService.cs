using Dqsm.Backend.Data;
using Dqsm.Backend.Models;

namespace Dqsm.Backend.Services
{
    /// <summary>
    /// Static Log Service Wrapper
    /// Implementation of the requirement: "创建一个静态日志服务...命名为Logs表"
    /// Uses ServiceLocator pattern internally to bridge Static -> DI
    /// </summary>
    public static class StaticLogService
    {
        private static IServiceProvider _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static void Log(string action, string infoType, string info)
        {
            if (_serviceProvider == null) return;

            try
            {
                // Create a new scope for the DB operation to avoid context threading issues
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    
                    var logEntry = new Logs
                    {
                        Action = action,
                        InfoType = infoType,
                        Info = info,
                        DateTime = DateTime.Now
                    };

                    dbContext.Logs.Add(logEntry);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Fallback to console logging if DB is not available (e.g. tables not yet created)
                Console.Error.WriteLine($"[StaticLogService] Failed to write log to DB: {ex.Message}");
            }
        }

        public static void Info(string action, string info) => Log(action, "info", info);
        public static void Debug(string action, string info) => Log(action, "debug", info);
        public static void Error(string action, string info) => Log(action, "error", info);
    }
}
