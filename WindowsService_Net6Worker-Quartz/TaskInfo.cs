using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService_Net6Worker_Quartz
{
    public class TaskInfo : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public TaskInfo(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<ITaskLogTime>();            
            await svc.DoWork(context.CancellationToken, _configuration);
            await Task.CompletedTask;
        }

    }
}
