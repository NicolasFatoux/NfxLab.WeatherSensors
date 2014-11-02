using System;
using Microsoft.SPOT;
using System.Collections;
using System.Diagnostics;

namespace NfxLab.MicroFramework.Logging
{
    public class Log
    {
        LogFormatter formatter = new LogFormatter();

        public string Name { get; set; }
        public int Level { get; set; }
        public IAppender[] Appenders { get; set; }

        public Log(params IAppender[] appenders)
        {
            this.Appenders = appenders;
        }

        public void Info(params object[] data)
        {
            Write(LogCategory.Info, data);
        }

        [Conditional("DEBUG")]
        public void Debug(params object[] data)
        {
            Write(LogCategory.Debug, data);
        }

        public void Warning(params object[] data)
        {
            Write(LogCategory.Warning, data);
        }

        public void Error(params object[] data)
        {
            Write(LogCategory.Error, data);
        }

        private void Write(LogCategory category, params object[] data)
        {
            string message = formatter.Format(category, data);

            foreach (IAppender appender in Appenders)
                appender.Write(message);
        }        
    }
}
