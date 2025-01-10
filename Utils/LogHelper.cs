using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WenElevating.FileShared.Utils
{
    public class LogHelper
    {
        private readonly log4net.ILog _operationLog;
        private readonly log4net.ILog _errorLog;

        public LogHelper()
        {
            _operationLog = log4net.LogManager.GetLogger("Operation");
            _errorLog = log4net.LogManager.GetLogger("Error");
        }

        public void Error(object message)
        {
            _errorLog.Error(message);
        }

        public void Info(object message)
        {
            _operationLog.Info(message);
        }
    }
}
