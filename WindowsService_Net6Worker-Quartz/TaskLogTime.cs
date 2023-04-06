using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService_Net6Worker_Quartz
{
    public class TaskLogTime : ITaskLogTime
    {
        public async Task DoWork(CancellationToken cancellationToken, IConfiguration config, ILogger logger)
        {
            await Execute(config, logger);
        }

        public async Task Execute(IConfiguration config, ILogger logger)
        {
            try
            {              
                // string path = @"C:\\Temp\\QuartzLogs.txt";

                var configKey = $"ConnectionStrings:LoggingOutputPath";
                var loggingPath = config[configKey];
                string path = loggingPath;

                //await using (StreamWriter writer = new StreamWriter(path, true))
                //{
                //    writer.WriteLine("Log Time: " + DateTime.Now);
                //    writer.Close();
                //}

                logger.LogInformation("Log Time from Serilog: " + DateTime.Now);
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Exception: " + ex.Message);
                logger.LogError(ex, "Error during Task Log Time");                
            }
        }

    }
}
