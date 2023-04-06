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

        public TaskInfo(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<ITaskLogTime>();
            await svc.DoWork(context.CancellationToken);
            await Task.CompletedTask;
        }

    }
}
