using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Tracing;

namespace StringDetectorService.Trace
{
    public class CustomizedTraceWriter : ITraceWriter
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CustomizedTraceWriter));

        string logFilePath = "C:\\inetpub\\logs\\LogFiles\\TraceLog[STARTTIME].log";

        public CustomizedTraceWriter()
        {
            logFilePath = logFilePath.Replace("[STARTTIME]", System.DateTime.Now.ToString("MM_dd_yyyy-HH_mm"));
        }

        public void Trace(System.Net.Http.HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            try
            {
                var record = new TraceRecord(request, category, level);
                traceAction(record);
                string message = String.Format("[{0,-5}]-[{1}]-[{2,-3}]-[{3,-5}]-[{4}]-[{5}]-[Message#{6}#]\r\n", record.Level, record.Timestamp, record.Status, record.Kind, record.Operator, record.Operation, record.Message);
                //File.AppendAllText(logFilePath, message);
                writeLog(level, message);
            }
            catch (Exception e)
            {
                //ignore
                Console.WriteLine(e.StackTrace);
            }
        }

        public void writeLog(TraceLevel level, string message)
        {
            switch (level)
            {
                case TraceLevel.Debug:
                    log.Debug(message); break;
                case TraceLevel.Error:
                    log.Error(message); break;
                case TraceLevel.Fatal:
                    log.Fatal(message); break;
                case TraceLevel.Info:
                    log.Info(message); break;
                case TraceLevel.Warn:
                    log.Warn(message); break;
                default:
                    log.Info(message); break;
            };
        }
    }
}