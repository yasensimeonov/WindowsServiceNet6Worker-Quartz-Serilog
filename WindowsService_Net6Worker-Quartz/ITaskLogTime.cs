﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService_Net6Worker_Quartz
{
    public interface ITaskLogTime
    {
        Task DoWork(CancellationToken cancellationToken, IConfiguration config, ILogger logger);
        Task Execute(IConfiguration config, ILogger logger);
    }
}
