using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService_Net6Worker_Quartz
{
    public class TaskLogTime : ITaskLogTime
    {
        public async Task DoWork(CancellationToken cancellationToken)
        {
            await Execute();
        }

        public async Task Execute()
        {
            try
            {
                string path = @"C:\\Temp\\QuartzLogs.txt";
                await using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine("Log Time: " + DateTime.Now);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

    }
}
